import { Component } from '@angular/core';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss'],
})
export class AuthFormComponent {
  formStateLogin: boolean = true;

  toggleFormStateLogin() {
    this.formStateLogin = true;
  }

  toggleFormStateRegister() {
    this.formStateLogin = false;
  }
}
