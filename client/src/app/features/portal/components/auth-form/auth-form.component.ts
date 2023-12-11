import { Component, OnInit } from '@angular/core';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { NgClass, NgIf } from '@angular/common';

@Component({
    selector: 'app-auth-form',
    templateUrl: './auth-form.component.html',
    styleUrls: ['./auth-form.component.scss'],
    standalone: true,
    imports: [
        NgClass,
        NgIf,
        LoginComponent,
        RegisterComponent,
    ],
})
export class AuthFormComponent implements OnInit {
  formStateLogin: boolean = true;

  ngOnInit(): void {
    this.loadFormState();
  }

  toggleFormStateLogin() {
    this.formStateLogin = true;
    localStorage.setItem('formState', 'login');
  }

  toggleFormStateRegister() {
    this.formStateLogin = false;
    localStorage.setItem('formState', 'register');
  }

  loadFormState() {
    const savedState = localStorage.getItem('formState');
    if (savedState) {
      this.formStateLogin = savedState === 'login';
    }
  }
}
