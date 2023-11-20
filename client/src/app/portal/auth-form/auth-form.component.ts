import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss'],
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
