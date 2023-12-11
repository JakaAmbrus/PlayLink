import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PortalComponent } from './portal.component';
import { AuthFormComponent } from './components/auth-form/auth-form.component';
import { LoginComponent } from './components/auth-form/login/login.component';
import { RegisterComponent } from './components/auth-form/register/register.component';
import { LoginLogoComponent } from './components/login-logo/login-logo.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

import { PortalRoutingModule } from './portal-routing.module';

@NgModule({
    imports: [
        CommonModule,
        PortalRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatAutocompleteModule,
        PortalComponent,
        AuthFormComponent,
        LoginComponent,
        RegisterComponent,
        LoginLogoComponent,
    ],
})
export class PortalModule {}
