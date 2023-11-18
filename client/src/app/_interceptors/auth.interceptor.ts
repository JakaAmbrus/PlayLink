import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private accountService: AccountService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    const token = this.accountService.getToken();

    if (
      token &&
      !request.url.endsWith('/account/login') &&
      !request.url.endsWith('/account/register')
    ) {
      
      const clonedReq = request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`),
      });

      return next.handle(clonedReq);
    }

    return next.handle(request);
  }
}
