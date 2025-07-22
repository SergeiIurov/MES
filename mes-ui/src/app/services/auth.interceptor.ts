import {HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';
import {inject} from '@angular/core';
import {AuthService} from './auth-service';

export const authInterceptor: HttpInterceptorFn = (request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  const authService = inject(AuthService);

  if (authService.token) {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${authService.token}`
      }
    });
  }
  return next(request);
};
