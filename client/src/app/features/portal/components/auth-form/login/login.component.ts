import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/shared/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  model: any = {};
  loggedIn: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {}
  guestLogin() {
    this.model.username = 'jakaambrus';
    this.model.password = 'ambrus123';
    this.login();
  }
  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.loggedIn = true;
        this.accountService.setLoggedIn(true);
        this.router.navigate(['/home']);
      },
      error: (error) => console.log(error),
    });
  }
}
