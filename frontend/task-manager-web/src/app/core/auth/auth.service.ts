import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { API_BASE_URL } from '../api/api.config';

const credentialsKey = 'task-manager.basicCredentials';
const usernameKey = 'task-manager.username';

interface CurrentUserResponse {
  username: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly credentials = signal<string | null>(sessionStorage.getItem(credentialsKey));
  private readonly currentUsername = signal<string | null>(sessionStorage.getItem(usernameKey));

  readonly isAuthenticated = computed(() => !!this.credentials());
  readonly username = computed(() => this.currentUsername());

  get authorizationHeader(): string | null {
    const credentials = this.credentials();
    return credentials ? `Basic ${credentials}` : null;
  }

  login(username: string, password: string): Observable<CurrentUserResponse> {
    const encoded = btoa(`${username}:${password}`);
    sessionStorage.setItem(credentialsKey, encoded);
    this.credentials.set(encoded);

    return this.http.get<CurrentUserResponse>(`${API_BASE_URL}/auth/me`).pipe(
      tap({
        next: user => {
          sessionStorage.setItem(usernameKey, user.username);
          this.currentUsername.set(user.username);
        },
        error: () => this.logout()
      })
    );
  }

  logout(): void {
    sessionStorage.removeItem(credentialsKey);
    sessionStorage.removeItem(usernameKey);
    this.credentials.set(null);
    this.currentUsername.set(null);
  }
}
