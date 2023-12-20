import { Component } from '@angular/core';
import { AccountService } from 'src/app/core/services/account.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LoginRequest } from 'src/app/shared/models/auth';
import { MatDialog } from '@angular/material/dialog';
import { GuestLoginDialogComponent } from '../guest-login-dialog/guest-login-dialog.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [FormsModule],
})
export class LoginComponent {
  loginData: LoginRequest = {
    username: '',
    password: '',
  };
  loggedIn: boolean = false;
  loading: boolean = false;

  constructor(
    private accountService: AccountService,
    private router: Router,
    public dialog: MatDialog
  ) {}

  guestLogin() {
    const dialogRef = this.dialog.open(GuestLoginDialogComponent);
    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'member') {
        this.loginData.username = 'jakaambrus';
        this.loginData.password = 'ambrus123';
        this.login();
      }
    });
  }
  login() {
    this.loading = true;
    this.accountService.login(this.loginData).subscribe({
      next: () => {
        this.loggedIn = true;
        this.loading = false;
        this.accountService.setLoggedIn(true);
        this.router.navigate(['/home']);
      },
      error: () => {
        this.loading = false;
      },
    });
  }
}
