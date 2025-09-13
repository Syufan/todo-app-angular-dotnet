// import { TestBed } from '@angular/core/testing';
// import { HttpErrorResponse } from '@angular/common/http';
// import { of, throwError } from 'rxjs';
// import { TodoStore } from './todo.store';
// import { TodoService } from './todo.service';
// import { TodoItemDto } from '../models/todo.models';

// describe('TodoStore', () => {
//   let store: TodoStore;
//   let api: jasmine.SpyObj<TodoService>;

//   beforeEach(() => {
//     api = jasmine.createSpyObj<TodoService>('TodoService', ['list', 'add', 'remove']);

//     TestBed.configureTestingModule({
//       providers: [
//         TodoStore,
//         { provide: TodoService, useValue: api },
//       ],
//     });

//     store = TestBed.inject(TodoStore);
//   });

//   it('initial state', () => {
//     expect(store.todos()).toEqual([]);
//     expect(store.loading()).toBeFalse();
//     expect(store.creating()).toBeFalse();
//     expect(store.error()).toBeNull();
//     expect(store.count()).toBe(0);
//   });

//   it('refresh(): success fills todos and clears flags', () => {
//     const mock: TodoItemDto[] = [
//       { id: 1, title: 'A' },
//       { id: 2, title: 'B' },
//     ];
//     api.list.and.returnValue(of(mock));

//     store.refresh();

//     expect(api.list).toHaveBeenCalled();
//     expect(store.loading()).toBeFalse();
//     expect(store.error()).toBeNull();
//     expect(store.todos()).toEqual(mock);
//     expect(store.count()).toBe(2);
//   });

//   it('refresh(): error sets error and resets loading', () => {
//     const err = new HttpErrorResponse({
//       status: 500,
//       error: { title: 'server boom' },
//     });
//     api.list.and.returnValue(throwError(() => err));

//     store.refresh();

//     expect(store.loading()).toBeFalse();
//     expect(store.error()).toBe('server boom');
//     expect(store.todos()).toEqual([]);
//   });

//   it('add(): success appends item and clears creating/error', () => {
//     const created: TodoItemDto = { id: 10, title: 'New' };
//     api.add.and.returnValue(of(created));

//     store.add('New');

//     expect(api.add).toHaveBeenCalledWith('New');
//     expect(store.creating()).toBeFalse();
//     expect(store.error()).toBeNull();
//     expect(store.todos()).toEqual([created]);
//     expect(store.count()).toBe(1);
//   });

//   it('add(): failure sets error and resets creating', () => {
//     const err = new HttpErrorResponse({
//       status: 400,
//       error: { title: 'Title cannot be empty.' },
//     });
//     api.add.and.returnValue(throwError(() => err));

//     store.add('   ');

//     expect(store.creating()).toBeFalse();
//     expect(store.error()).toBe('Title cannot be empty.');
//     expect(store.todos()).toEqual([]);
//   });

//   it('remove(): success filters list', () => {
//     // seed state
//     (store as any).todos.set([
//       { id: 1, title: 'A' },
//       { id: 2, title: 'B' },
//     ]);
//     api.remove.and.returnValue(of(void 0));

//     store.remove(1);

//     expect(api.remove).toHaveBeenCalledWith(1);
//     expect(store.todos()).toEqual([{ id: 2, title: 'B' }]);
//     expect(store.count()).toBe(1);
//   });

//   it('remove(): failure keeps list and sets error', () => {
//     (store as any).todos.set([{ id: 1, title: 'A' }]);
//     const err = new HttpErrorResponse({ status: 500, error: 'boom' });
//     api.remove.and.returnValue(throwError(() => err));

//     store.remove(1);

//     expect(store.todos()).toEqual([{ id: 1, title: 'A' }]);
//     expect(store.error()).toBe('boom');
//   });
// });