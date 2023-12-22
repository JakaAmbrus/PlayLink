import { Component } from '@angular/core';
import { AccountService } from 'src/app/core/services/account.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LoginRequest } from 'src/app/shared/models/auth';
import { MatDialog } from '@angular/material/dialog';
import { GuestLoginDialogComponent } from '../guest-login-dialog/guest-login-dialog.component';
import { first } from 'rxjs';

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
  isLoading: boolean = false;

  constructor(
    private accountService: AccountService,
    private router: Router,
    public dialog: MatDialog
  ) {}

  guestLogin(): void {
    const dialogRef = this.dialog.open(GuestLoginDialogComponent);
    dialogRef
      .afterClosed()
      .pipe(first())
      .subscribe((result) => {
        const role = result;
        if (role) {
          this.isLoading = true;
          this.accountService.guestLogin(role).subscribe({
            next: () => {
              this.loggedIn = true;
              this.isLoading = false;
              this.accountService.setLoggedIn(true);
              this.router.navigate(['/home']);
            },
            error: () => {
              this.isLoading = false;
            },
          });
        }
      });
  }

  login(): void {
    this.isLoading = true;
    this.accountService
      .login(this.loginData)
      .pipe(first())
      .subscribe({
        next: () => {
          this.loggedIn = true;
          this.isLoading = false;
          this.accountService.setLoggedIn(true);
          this.router.navigate(['/home']);
        },
        error: () => {
          this.isLoading = false;
        },
      });
  }
}
