import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { correlationIdInterceptor } from './interceptors/correlation-id.interceptor';
import { problemDetailsInterceptor } from './interceptors/problem-details.interceptor';
import { routes } from './app.routes';
import { API_BASE_URL } from './core/tokens';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),

    provideHttpClient(
      withFetch(),
      withInterceptors([
        correlationIdInterceptor,
        problemDetailsInterceptor,
      ])
    ),

    { provide: API_BASE_URL, useValue: environment.apiBaseUrl },
  ],
};
