import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../core/auth/auth.service';
import { SaveTaskRequest, TaskDto, TaskStatus } from '../../core/tasks/task.models';
import { TaskService } from '../../core/tasks/task.service';
import { TaskFormComponent } from './task-form/task-form.component';
import { TaskListComponent } from './task-list/task-list.component';

@Component({
  selector: 'app-tasks-page',
  standalone: true,
  imports: [TaskListComponent, TaskFormComponent],
  templateUrl: './tasks-page.component.html',
  styleUrl: './tasks-page.component.css'
})
export class TasksPageComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly tasksApi = inject(TaskService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly tasks = signal<TaskDto[]>(this.route.snapshot.data['tasks'] ?? []);
  readonly selectedTask = signal<TaskDto | null>(null);
  readonly isSaving = signal(false);
  readonly message = signal<string | null>(null);
  readonly error = signal<string | null>(null);
  readonly username = this.auth.username;

  readonly completionSummary = computed(() => {
    const all = this.tasks();
    const done = all.filter(t => t.status === TaskStatus.Done || t.isCompleted).length;
    return { total: all.length, done };
  });

  onTaskSelect(task: TaskDto): void {
    this.selectedTask.set(task);
  }

  onNewTask(): void {
    this.selectedTask.set(null);
  }

  onSave(request: SaveTaskRequest): void {
    this.isSaving.set(true);
    this.error.set(null);

    const selected = this.selectedTask();
    const op = selected
      ? this.tasksApi.updateTask(selected.id, request)
      : this.tasksApi.createTask(request);

    op.pipe(finalize(() => this.isSaving.set(false))).subscribe({
      next: saved => {
        this.upsertTask(saved);
        this.selectedTask.set(saved);
        this.showMessage(selected ? 'Task updated.' : 'Task created.');
      },
      error: () => this.error.set('Could not save the task.')
    });
  }

  onStatusChange(event: { task: TaskDto; status: TaskStatus }): void {
    this.tasksApi.changeStatus(event.task.id, event.status).subscribe({
      next: updated => {
        this.upsertTask(updated);
        if (this.selectedTask()?.id === updated.id) this.selectedTask.set(updated);
        this.showMessage('Task status changed.');
      },
      error: () => this.error.set('Could not update task status.')
    });
  }

  onTaskComplete(task: TaskDto): void {
    if (task.isCompleted || task.status === TaskStatus.Done) return;

    this.tasksApi.completeTask(task.id).subscribe({
      next: updated => {
        this.upsertTask(updated);
        if (this.selectedTask()?.id === updated.id) this.selectedTask.set(updated);
        this.showMessage('Task completed.');
      },
      error: () => this.error.set('Could not complete task.')
    });
  }

  onTaskDelete(task: TaskDto): void {
    if (!window.confirm(`Delete "${task.title}"?`)) return;

    this.tasksApi.deleteTask(task.id).subscribe({
      next: () => {
        this.tasks.update(all => all.filter(t => t.id !== task.id));
        if (this.selectedTask()?.id === task.id) this.selectedTask.set(null);
        this.showMessage('Task deleted.');
      },
      error: () => this.error.set('Could not delete task.')
    });
  }

  onClear(): void {
    this.selectedTask.set(null);
  }

  logout(): void {
    this.auth.logout();
    this.router.navigateByUrl('/login');
  }

  private upsertTask(task: TaskDto): void {
    this.tasks.update(all => {
      const exists = all.some(t => t.id === task.id);
      return exists ? all.map(t => t.id === task.id ? task : t) : [task, ...all];
    });
  }

  private showMessage(msg: string): void {
    this.message.set(msg);
    window.setTimeout(() => this.message.set(null), 2600);
  }
}
