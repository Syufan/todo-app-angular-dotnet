// client/src/app/services/todo.service.ts
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/tokens';
import { CreateTodoRequest, TodoItemDto } from '../models/todo.models';

@Injectable({ providedIn: 'root' })
export class TodoService {
  private readonly http = inject(HttpClient);
  private readonly base = inject(API_BASE_URL);

  list(): Observable<TodoItemDto[]> {
    return this.http.get<TodoItemDto[]>(`${this.base}/todo`);
  }
  add(title: string): Observable<TodoItemDto> {
    const body: CreateTodoRequest = { title };
    return this.http.post<TodoItemDto>(`${this.base}/todo`, body);
  }
  remove(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/todo/${id}`);
  }
}

