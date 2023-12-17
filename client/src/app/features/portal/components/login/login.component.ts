import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/core/services/account.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LoginRequest } from 'src/app/shared/models/auth';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [FormsModule],
})
export class LoginComponent implements OnInit {
  loginData: LoginRequest = {
    username: '',
    password: '',
  };
  loggedIn: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {}
  guestLogin() {
    this.loginData.username = 'jakaambrus';
    this.loginData.password = 'ambrus123';
    this.login();
  }
  login() {
    this.accountService.login(this.loginData).subscribe({
      next: () => {
        this.loggedIn = true;
        this.accountService.setLoggedIn(true);
        this.router.navigate(['/home']);
      },
    });
  }
}
