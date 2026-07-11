import { DatePipe } from '@angular/common';
import { Component, computed, input, output, signal } from '@angular/core';
import { TaskDto, TaskPriority, TaskStatus } from '../../../core/tasks/task.models';

type SortField = 'createdAt' | 'dueDate' | 'title' | 'priority' | 'status';
type SortDirection = 'asc' | 'desc';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent {
  readonly tasks = input<TaskDto[]>([]);
  readonly selectedTaskId = input<string | null>(null);

  readonly taskSelect = output<TaskDto>();
  readonly taskComplete = output<TaskDto>();
  readonly taskDelete = output<TaskDto>();
  readonly newTask = output<void>();

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

  readonly search = signal('');
  readonly statusFilter = signal<'all' | TaskStatus>('all');
  readonly priorityFilter = signal<'all' | TaskPriority>('all');
  readonly sortField = signal<SortField>('createdAt');
  readonly sortDirection = signal<SortDirection>('desc');

  readonly filteredTasks = computed(() => {
    const search = this.search().trim().toLowerCase();
    const status = this.statusFilter();
    const priority = this.priorityFilter();
    const field = this.sortField();
    const direction = this.sortDirection();

    return this.tasks()
      .filter(task => {
        const matchesSearch = !search
          || task.title.toLowerCase().includes(search)
          || (task.description ?? '').toLowerCase().includes(search);
        const matchesStatus = status === 'all' || task.status === status;
        const matchesPriority = priority === 'all' || task.priority === priority;
        return matchesSearch && matchesStatus && matchesPriority;
      })
      .sort((a, b) => this.compareTasks(a, b, field, direction));
  });

  updateSearch(value: string): void { this.search.set(value); }

  updateStatusFilter(value: string): void {
    this.statusFilter.set(value === 'all' ? 'all' : Number(value) as TaskStatus);
  }

  updatePriorityFilter(value: string): void {
    this.priorityFilter.set(value === 'all' ? 'all' : Number(value) as TaskPriority);
  }

  updateSortField(value: string): void { this.sortField.set(value as SortField); }

  toggleSortDirection(): void {
    this.sortDirection.update(d => d === 'asc' ? 'desc' : 'asc');
  }

  priorityLabel(priority: TaskPriority): string {
    return this.priorities.find(p => p.value === priority)?.label ?? 'Unknown';
  }

  statusLabel(status: TaskStatus): string {
    return this.statuses.find(s => s.value === status)?.label ?? 'Unknown';
  }

  private compareTasks(a: TaskDto, b: TaskDto, field: SortField, direction: SortDirection): number {
    const mult = direction === 'asc' ? 1 : -1;
    const av = this.sortValue(a, field);
    const bv = this.sortValue(b, field);
    return av > bv ? mult : av < bv ? -mult : 0;
  }

  private sortValue(task: TaskDto, field: SortField): string | number {
    if (field === 'dueDate') return task.dueDate ? new Date(task.dueDate).getTime() : Number.MAX_SAFE_INTEGER;
    if (field === 'createdAt') return new Date(task.createdAt).getTime();
    return task[field];
  }
}
