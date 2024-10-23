import { Component } from '@angular/core';
import { AuthFormComponent } from './components/auth-form/auth-form.component';
import { LoginLogoComponent } from './components/login-logo/login-logo.component';

@Component({
  selector: 'app-portal',
  templateUrl: './portal.component.html',
  styleUrls: ['./portal.component.scss'],
  standalone: true,
  imports: [LoginLogoComponent, AuthFormComponent],
})
export class PortalComponent {}
