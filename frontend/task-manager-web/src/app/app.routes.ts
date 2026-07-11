import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { tasksResolver } from './core/tasks/tasks.resolver';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/login/login.component').then(c => c.LoginComponent)
  },
  {
    path: 'tasks',
    canActivate: [authGuard],
    resolve: { tasks: tasksResolver },
    loadComponent: () => import('./features/tasks/tasks-page.component').then(c => c.TasksPageComponent)
  },
  { path: '', pathMatch: 'full', redirectTo: 'tasks' },
  { path: '**', redirectTo: 'tasks' }
];
