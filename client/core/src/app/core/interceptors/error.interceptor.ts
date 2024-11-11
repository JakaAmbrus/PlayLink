import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest,} from '@angular/common/http';
import {catchError, Observable} from 'rxjs';
import {ToastrService} from 'ngx-toastr';
import {AccountService} from "../services/account.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService, private accountService: AccountService,) {
  }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: any) => {
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
                this.toastr.error(error.error.message || 'Bad Request');
              }
              break;

            case 401:
              this.toastr.error(error.error.message || 'Unauthorized');
              break;

            case 404:
              this.toastr.error(error.error.message || 'Not Found');
              break;

            case 409:
              this.toastr.error(error.error.message || 'Conflict');
              break;

            case 429:
              this.toastr.error(
                error.error.message ||
                'Maximum hourly limit for feature reached.'
              );
              break;

            case 498: // When token expires I just log them out, for simplicity since this is a once visit in a while app
              this.accountService.logout();
              break;

            case 500:
              this.toastr.error(error.error.message || 'Something went wrong..');
              break;

            default:
              this.toastr.error('Something went wrong..');
              break;
          }
        }
        throw error;
      })
    );
  }
}
