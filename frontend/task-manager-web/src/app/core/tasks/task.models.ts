export enum TaskPriority {
  Low = 0,
  Medium = 1,
  High = 2
}

export enum TaskStatus {
  Todo = 0,
  InProgress = 1,
  Done = 2
}

export interface TaskDto {
  id: string;
  title: string;
  description: string | null;
  priority: TaskPriority;
  status: TaskStatus;
  isCompleted: boolean;
  dueDate: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface SaveTaskRequest {
  title: string;
  description: string | null;
  priority: TaskPriority;
  dueDate: string | null;
}
