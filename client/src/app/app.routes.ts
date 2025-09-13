import { Routes } from '@angular/router';
import { TodosPage } from './pages/todos/todos.page';

export const routes: Routes = [
  { path: '', component: TodosPage },
  { path: '**', redirectTo: '' },
];