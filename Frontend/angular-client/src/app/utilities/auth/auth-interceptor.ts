import { Injectable, inject } from '@angular/core';
import {
  HttpRequest,
  HttpErrorResponse,
  HttpInterceptorFn,
  HttpHandlerFn
} from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';


export const authInterceptor: HttpInterceptorFn = (request: HttpRequest<any>, next: HttpHandlerFn) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const accessToken = localStorage.getItem('accessToken');
  if (accessToken) {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return authService.refreshAccessToken().pipe(
          switchMap(response => {
            const newRequest = request.clone({
              setHeaders: {
                Authorization: `Bearer ${response.accessToken}`
              }
            });
            return next(newRequest);
          }),
          catchError(refreshError => {
            authService.logout();
            router.navigate(['/sign-in']);
            return throwError(() => refreshError);
          })
        );
      }

      return throwError(() => error);
    })
  );
};
