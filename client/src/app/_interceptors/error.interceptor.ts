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
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                for (const err of modelStateErrors) {
                  this.toastr.error(err);
                }
                //if I decide to not show this error with toastr, maybe I want to handle it while making the form in the future
                //throw modelStateErrors.flat();
              } else {
                this.toastr.error(error.error, error.status.toString());
              }
              break;

            case 401:
              this.toastr.error('Unauthorized', error.status.toString());
              break;

            case 404:
              this.router.navigateByUrl('/not-found');
              break;

            case 500:
              const navigationExtras: NavigationExtras = {
                state: { error: error.error },
              };
              this.router.navigateByUrl('/server-error', navigationExtras);
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
