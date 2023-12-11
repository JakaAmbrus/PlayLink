import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../../../../../shared/services/account.service';

@Component({
    selector: 'app-header-dropdown',
    templateUrl: './header-dropdown.component.html',
    styleUrls: ['./header-dropdown.component.scss'],
    standalone: true,
})
export class HeaderDropdownComponent {
  constructor(private accountService: AccountService, private router: Router) {}
  @Input() username: string | null = null;

  navigateToEditProfile() {
    this.router
      .navigateByUrl('/RefreshComponent', { skipLocationChange: true })
      .then(() => {
        this.router.navigate(['/user', this.username, 'edit']);
      });
  }

  logout() {
    this.accountService.logout();
    this.router.navigate(['/portal']);
  }
}
