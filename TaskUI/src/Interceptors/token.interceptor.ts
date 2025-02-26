import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, throwError, switchMap } from 'rxjs';
import { AuthService } from '../Services/auth.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const myToken = this.auth.getToken();

    if (myToken) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${myToken}` }
      });
    }

    return next.handle(request).pipe(
      catchError((err: any) => {
        if (err instanceof HttpErrorResponse && err.status === 401) {
          return this.handleUnAuthorizedError(request, next);
        }
        return throwError(() => err);
      })
    );
  }

  handleUnAuthorizedError(req: HttpRequest<any>, next: HttpHandler) {
    const refreshToken = this.auth.getRefreshToken();
    if (refreshToken) {
        return this.auth.refreshToken(refreshToken).pipe(
            switchMap((response: any) => {
                this.auth.storeToken(response.token);
                req = req.clone({
                    setHeaders: { Authorization: `Bearer ${response.token}` }
                });
                return next.handle(req);
            }),
            catchError((error) => {
                this.auth.logout();
                return throwError(() => error);
            })
        );
    } else {
        this.auth.logout();
        return throwError(() => new Error('No refresh token found'));
    }
}
}
