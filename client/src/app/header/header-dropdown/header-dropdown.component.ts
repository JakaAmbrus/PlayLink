import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-header-dropdown',
  templateUrl: './header-dropdown.component.html',
  styleUrls: ['./header-dropdown.component.scss'],
})
export class HeaderDropdownComponent {
  constructor(private accountService: AccountService, private router: Router) {}

  logout() {
    this.accountService.logout();
    this.router.navigate(['/portal']);
  }
}
