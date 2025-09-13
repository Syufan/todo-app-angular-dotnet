// client/src/app/services/todo.store.ts
import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../core/tokens';
import { CreateTodoRequest, TodoItemDto } from '../models/todo.models';

// Service decorator
@Injectable({ providedIn: 'root' })
export class TodoStore {
  private readonly http = inject(HttpClient);
  private readonly base = inject(API_BASE_URL);

  // Reactive state management using Angular Signals
  todos   = signal<TodoItemDto[]>([]);
  loading = signal(false);
  error   = signal<string | null>(null);

  // Fetch all todos from backend API
  refresh() {
    this.loading.set(true);
    this.http.get<TodoItemDto[]>(`${this.base}/todo`).subscribe({
      next: list => { this.todos.set(list); this.loading.set(false); },
      error: err => { this.error.set(this.readErr(err)); this.loading.set(false); },
    });
  }

  // Create a new todo item
  add(title: string) {
    const body: CreateTodoRequest = { title };
    this.http.post<TodoItemDto>(`${this.base}/todo`, body).subscribe({
      next: item => this.todos.update(arr => [...arr, item]),
      error: err => this.error.set(this.readErr(err)),
    });
  }

  // Delete a todo item by ID
  remove(id: number) {
    this.http.delete<void>(`${this.base}/todo/${id}`).subscribe({
      next: () => this.todos.update(arr => arr.filter(x => x.id !== id)),
      error: err => this.error.set(this.readErr(err)),
    });
  }

  // To extract error message
  private readErr(e: any): string {
    return e?.error?.title || e?.error?.error || e?.message || 'Unknown error';
  }
}