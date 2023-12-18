import { Component, Input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../../../services/account.service';

@Component({
  selector: 'app-header-dropdown',
  templateUrl: './header-dropdown.component.html',
  styleUrls: ['./header-dropdown.component.scss'],
  standalone: true,
  imports: [RouterLink],
})
export class HeaderDropdownComponent {
  @Input() username: string | null = null;

  constructor(private accountService: AccountService, private router: Router) {}

  logout() {
    this.accountService.logout();
    this.router.navigate(['/portal']);
  }
}
