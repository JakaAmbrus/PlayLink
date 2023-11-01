import { Component } from '@angular/core';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent {
  formStateLogin: boolean = true;

  toggleFormStateLogin() {
    this.formStateLogin = true;
  }

  toggleFormStateRegister() {
    this.formStateLogin = false;
  }
}
