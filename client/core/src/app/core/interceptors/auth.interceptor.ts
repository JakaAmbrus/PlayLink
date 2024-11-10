import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest,} from '@angular/common/http';
import {Observable} from 'rxjs';
import {TokenService} from '../services/token.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private tokenService: TokenService) {
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    const token = this.tokenService.getToken();

    if (
      token &&
      !request.url.endsWith('/users/current') &&
      !request.url.endsWith('/shield/guest') &&
      !request.url.endsWith('/shield/signup')
    ) {
      const clonedReq = request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`),
      });

      return next.handle(clonedReq);
    }

    return next.handle(request);
  }
}
