import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api/api.config';
import { SaveTaskRequest, TaskDto, TaskStatus } from './task.models';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private readonly http = inject(HttpClient);
  private readonly url = `${API_BASE_URL}/tasks`;

  getTasks(): Observable<TaskDto[]> {
    return this.http.get<TaskDto[]>(this.url);
  }

  createTask(request: SaveTaskRequest): Observable<TaskDto> {
    return this.http.post<TaskDto>(this.url, request);
  }

  updateTask(id: string, request: SaveTaskRequest): Observable<TaskDto> {
    return this.http.put<TaskDto>(`${this.url}/${id}`, request);
  }

  completeTask(id: string): Observable<TaskDto> {
    return this.http.patch<TaskDto>(`${this.url}/${id}/complete`, {});
  }

  changeStatus(id: string, status: TaskStatus): Observable<TaskDto> {
    return this.http.patch<TaskDto>(`${this.url}/${id}/status`, { status });
  }

  deleteTask(id: string): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
