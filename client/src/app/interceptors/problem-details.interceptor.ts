import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const problemDetailsInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      const pd = err.error as
        | { title?: string; detail?: string; status?: number; errors?: Record<string, string[]> }
        | undefined;

      console.error('API error', {
        url: req.url,
        status: err.status,
        title: pd?.title ?? err.statusText,
        detail: pd?.detail,
        errors: pd?.errors,
      });

      return throwError(() => err);
    })
  );