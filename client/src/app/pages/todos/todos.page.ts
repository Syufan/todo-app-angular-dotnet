import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { TodoStore } from '../../services/todo.store';

@Component({
  standalone: true,
  selector: 'app-todos',
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <section class="w-full max-w-2xl mx-auto mt-40">
      <h2 class="mb-6 text-3xl font-semibold tracking-tight">Todos</h2>

      <form (submit)="onCreate($event)" class="mb-4 flex gap-3">
        <input
          [formControl]="title"
          type="text"
          maxlength="200"
          placeholder="What needs to be done?"
          class="flex-1 rounded-xl border border-slate-300 px-4 py-2
                 placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-indigo-500" />

        <button
          type="submit"
          [disabled]="title.invalid || store.creating()"
          class="rounded-xl bg-indigo-600 px-4 py-2 font-medium text-white shadow-sm transition
                 hover:bg-indigo-500 disabled:cursor-not-allowed disabled:opacity-50">
          Add
        </button>
      </form>

      <ul class="divide-y divide-slate-200 rounded-xl border border-slate-200 bg-white/60 backdrop-blur">
        <li *ngFor="let t of store.todos()" class="flex items-center justify-between gap-3 px-4 py-3">
          <span class="select-text text-slate-900">{{ t.title }}</span>
          <button
            type="button"
            (click)="store.remove(t.id)"
            aria-label="Delete"
            class="inline-flex items-center rounded-lg bg-slate-100 px-2 py-1 text-sm text-rose-600
                   ring-1 ring-rose-200 hover:bg-rose-50 hover:text-rose-500 focus:outline-none focus:ring-2 focus:ring-rose-300">
            ×
          </button>
        </li>
      </ul>

      <div class="mt-3 flex items-center gap-3" *ngIf="store.loading() || store.error()">
        <div *ngIf="store.loading()" class="inline-flex items-center gap-2 text-slate-500">
          <span class="h-4 w-4 animate-spin rounded-full border-2 border-slate-400 border-t-transparent"></span>
          Loading…
        </div>
        <p *ngIf="store.error()" class="text-rose-600">{{ store.error() }}</p>
      </div>

      <p class="mt-3 text-sm text-slate-500">Total: {{ store.count() }}</p>
    </section>
  `,
})
export class TodosPage implements OnInit {  
  readonly store = inject(TodoStore);
  title = new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(200)] });
  ngOnInit() { this.store.refresh(); }
  onCreate(ev?: Event) {
    ev?.preventDefault();
    const v = (this.title.value || '').trim();
    if (!v) { this.title.setErrors({ required: true }); return; }
    this.store.add(v);
    this.title.reset('');
  }
}