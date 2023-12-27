import { Component, OnInit } from '@angular/core';
import { RegisterComponent } from '../register/register.component';
import { LoginComponent } from '../login/login.component';
import { NgClass } from '@angular/common';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss'],
  standalone: true,
  imports: [NgClass, LoginComponent, RegisterComponent],
})
export class AuthFormComponent implements OnInit {
  formStateLogin: boolean = true;

  constructor(private localStorageService: LocalStorageService) {}

  ngOnInit(): void {
    this.loadFormState();
  }

  toggleFormStateLogin() {
    this.formStateLogin = true;
    this.localStorageService.setItem('formState', 'login');
  }

  toggleFormStateRegister() {
    this.formStateLogin = false;
    this.localStorageService.setItem('formState', 'register');
  }

  loadFormState() {
    const savedState = this.localStorageService.getItem('formState');
    if (savedState) {
      this.formStateLogin = savedState === 'login';
    }
  }
}
