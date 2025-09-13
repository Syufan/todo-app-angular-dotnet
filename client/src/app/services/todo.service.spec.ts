import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TodoService } from './todo.service';
import { API_BASE_URL } from '../core/tokens';
import { TodoItemDto } from '../models/todo.models';

describe('TodoService', () => {
  let svc: TodoService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: API_BASE_URL, useValue: '/api' }],
    });

    svc = TestBed.inject(TodoService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('list(): calls GET /api/todo and returns items', () => {
    const mock: TodoItemDto[] = [
      { id: 1, title: 'Buy milk' },
      { id: 2, title: 'Read book' },
    ];

    let data: TodoItemDto[] | undefined;
    svc.list().subscribe(v => (data = v));

    const req = http.expectOne(r => r.method === 'GET' && r.url === '/api/todo');
    expect(req.request.method).toBe('GET');
    req.flush(mock);

    expect(data).toEqual(mock);
  });

  it('add(): calls POST /api/todo with body and returns created item', () => {
    const created: TodoItemDto = { id: 99, title: 'New' };

    let data: TodoItemDto | undefined;
    svc.add('New').subscribe(v => (data = v));

    const req = http.expectOne(r => r.method === 'POST' && r.url === '/api/todo');
    expect(req.request.body).toEqual({ title: 'New' });
    req.flush(created);

    expect(data).toEqual(created);
  });

  it('remove(): calls DELETE /api/todo/{id}', () => {
    let completed = false;
    svc.remove(42).subscribe({ complete: () => (completed = true) });

    const req = http.expectOne(r => r.method === 'DELETE' && r.url === '/api/todo/42');
    req.flush(null);
    expect(completed).toBeTrue();
  });
});