import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TodoStore } from './services/todo.store';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App implements OnInit {
  private readonly store = inject(TodoStore);

  todos   = this.store.todos;
  loading = this.store.loading;
  error   = this.store.error;

  newTitle = '';

  ngOnInit(): void {
    this.store.refresh();
  }
  
  trackById = (_: number, t: { id: number }) => t.id;

  add(): void {
    const t = this.newTitle.trim();
    if (!t) return;
    this.store.add(t);
    this.newTitle = '';
  }

  remove(id: number): void {
    this.store.remove(id);
  }
}