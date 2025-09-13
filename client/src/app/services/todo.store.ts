// client/src/app/services/todo.store.ts
import { Injectable, inject, signal, computed } from '@angular/core';
import { TodoService } from './todo.service';
import { TodoItemDto } from '../models/todo.models';

// Service decorator
@Injectable({ providedIn: 'root' })
export class TodoStore {
  private readonly api = inject(TodoService);

  // Reactive state management using Angular Signals
  readonly todos    = signal<TodoItemDto[]>([]);
  readonly loading  = signal(false);
  readonly creating = signal(false);
  readonly error    = signal<string | null>(null);
  readonly count    = computed(() => this.todos().length);

  // Fetch all todos from backend API
  refresh() {
    this.loading.set(true);
    this.api.list().subscribe({
      next: list => { this.todos.set(list); this.loading.set(false); this.error.set(null); },
      error: err  => { this.loading.set(false); this.error.set(this.readErr(err)); }
    });
  }

  // Create a new todo item
  add(title: string) {
    this.creating.set(true);
    this.api.add(title).subscribe({
      next: item => { this.todos.update(a => [...a, item]); this.creating.set(false); this.error.set(null); },
      error: err  => { this.creating.set(false); this.error.set(this.readErr(err)); }
    });
  }

  // Delete a todo item by ID
  remove(id: number) {
    this.api.remove(id).subscribe({
      next: () => this.todos.update(a => a.filter(x => x.id !== id)),
      error: err => this.error.set(this.readErr(err)),
    });
  }

  // To extract error message
  private readErr(e: any): string {
    return e?.error?.title ?? e?.error?.error ?? e?.message ?? 'Unknown error';
  }
}