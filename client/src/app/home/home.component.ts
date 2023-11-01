import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  constructor(private accountService: AccountService, private router: Router) {}

  logout() {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }
}
