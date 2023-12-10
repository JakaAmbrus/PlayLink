import { NgModule } from '@angular/core';
import { CommonModule, provideCloudinaryLoader } from '@angular/common';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    [provideCloudinaryLoader('https://res.cloudinary.com/dsdleukb7')],
  ],
})
export class CoreModule {}
