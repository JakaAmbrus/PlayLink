import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                const modelStateErrors = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStateErrors.push(...error.error.errors[key]);
                  }
                }
                modelStateErrors.forEach((err) => {
                  this.toastr.error(err);
                });
              } else {
                this.toastr.error(error.error, error.status.toString());
              }
              break;
            case 401:
              if (error.error.message) {
                this.toastr.error(error.error.message);
              } else {
                this.toastr.error('Unauthorized', error.status.toString());
              }
              break;

            case 404:
              this.router.navigateByUrl('/not-found');
              break;

            case 500:
              //was thinking about adding a server error page, but I don't think it's necessary, will leave her for future if I change my mind
              // const navigationExtras: NavigationExtras = {
              //   state: { error: error.error },
              // };
              // this.router.navigateByUrl('/server-error', navigationExtras);
              this.toastr.error('Server Error', error.status.toString());
              break;

            default:
              this.toastr.error('Something went wrong..');
              console.log(error);
              break;
          }
        }
        throw error;
      })
    );
  }
}
