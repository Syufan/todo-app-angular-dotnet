import { HttpInterceptorFn } from '@angular/common/http';

export const correlationIdInterceptor: HttpInterceptorFn = (req, next) => {
  const id = (globalThis.crypto?.randomUUID?.() ?? Date.now().toString(36));
  return next(req.clone({ setHeaders: { 'X-Correlation-Id': id } }));
};