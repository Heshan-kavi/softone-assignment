import { Component, effect, inject, input, output } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { SaveTaskRequest, TaskDto, TaskPriority, TaskStatus } from '../../../core/tasks/task.models';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.css'
})
export class TaskFormComponent {
  private readonly fb = inject(FormBuilder);

  readonly selectedTask = input<TaskDto | null>(null);
  readonly isSaving = input<boolean>(false);

  readonly save = output<SaveTaskRequest>();
  readonly statusChange = output<{ task: TaskDto; status: TaskStatus }>();
  readonly clear = output<void>();

  readonly TaskPriority = TaskPriority;
  readonly TaskStatus = TaskStatus;

  readonly priorities = [
    { value: TaskPriority.Low, label: 'Low' },
    { value: TaskPriority.Medium, label: 'Medium' },
    { value: TaskPriority.High, label: 'High' }
  ];

  readonly statuses = [
    { value: TaskStatus.Todo, label: 'Todo' },
    { value: TaskStatus.InProgress, label: 'In progress' },
    { value: TaskStatus.Done, label: 'Done' }
  ];

  readonly today = new Date().toISOString().substring(0, 10);

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    priority: [TaskPriority.Medium, Validators.required],
    dueDate: ['', this.futureDateValidator]
  });

  private futureDateValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;
    const today = new Date().toISOString().substring(0, 10);
    return control.value < today ? { pastDate: true } : null;
  }

  constructor() {
    effect(() => {
      const task = this.selectedTask();
      this.form.reset({
        title: task?.title ?? '',
        description: task?.description ?? '',
        priority: task?.priority ?? TaskPriority.Medium,
        dueDate: task?.dueDate ? task.dueDate.substring(0, 10) : ''
      });
    });
  }

  submit(): void {
    if (this.form.invalid || this.isSaving()) {
      this.form.markAllAsTouched();
      return;
    }
    this.save.emit(this.toSaveRequest());
  }

  setStatus(status: TaskStatus): void {
    const task = this.selectedTask();
    if (task && task.status !== status) {
      this.statusChange.emit({ task, status });
    }
  }

  private toSaveRequest(): SaveTaskRequest {
    const v = this.form.getRawValue();
    return {
      title: v.title.trim(),
      description: v.description.trim() || null,
      priority: Number(v.priority) as TaskPriority,
      dueDate: v.dueDate ? new Date(`${v.dueDate}T00:00:00`).toISOString() : null
    };
  }
}
