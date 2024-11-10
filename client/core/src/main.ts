import {importProvidersFrom} from '@angular/core';
import {AppComponent} from './app/app.component';
import {ToastrModule} from 'ngx-toastr';
import {TimeagoModule} from 'ngx-timeago';
import {NgOptimizedImage} from '@angular/common';
import {InfiniteScrollModule} from 'ngx-infinite-scroll';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import {NgxPaginationModule} from 'ngx-pagination';
import {NgxDropzoneModule} from 'ngx-dropzone';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatNativeDateModule} from '@angular/material/core';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {provideAnimations} from '@angular/platform-browser/animations';
import {AppRoutingModule} from './app/app-routing.module';
import {CoreModule} from './app/core/core.module';
import {bootstrapApplication, BrowserModule} from '@angular/platform-browser';
import {initializeApp, provideFirebaseApp} from "@angular/fire/app";
import {getAuth, provideAuth} from "@angular/fire/auth";
import {provideHttpClient} from "@angular/common/http";
import {firebaseConfig} from "./app/firebaseConfig";

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(
      BrowserModule,
      CoreModule,
      AppRoutingModule,
      FormsModule,
      ReactiveFormsModule,
      MatDatepickerModule,
      MatInputModule,
      MatFormFieldModule,
      MatNativeDateModule,
      MatAutocompleteModule,
      NgxDropzoneModule,
      NgxPaginationModule,
      MatButtonToggleModule,
      InfiniteScrollModule,
      NgOptimizedImage,
      TimeagoModule.forRoot(),
      ToastrModule.forRoot({
        timeOut: 4000,
        positionClass: 'toast-bottom-right',
        preventDuplicates: true,
        resetTimeoutOnDuplicate: true,
        progressBar: true,
      })
    ),
    provideHttpClient(),
    provideFirebaseApp(() => initializeApp(firebaseConfig)),
    provideAuth(() => getAuth()),
    provideAnimations(),
  ],
}).catch((err) => console.error(err));
