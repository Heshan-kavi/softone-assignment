import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { catchError, of } from 'rxjs';
import { TaskDto } from './task.models';
import { TaskService } from './task.service';

export const tasksResolver: ResolveFn<TaskDto[]> = () =>
  inject(TaskService).getTasks().pipe(catchError(() => of([])));
