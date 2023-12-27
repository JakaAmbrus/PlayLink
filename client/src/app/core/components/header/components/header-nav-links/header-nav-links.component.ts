import { Component } from '@angular/core';
import { navigationLinks } from './nav-links';
import { RouterLinkActive, RouterLink } from '@angular/router';

@Component({
  selector: 'app-header-nav-links',
  templateUrl: './header-nav-links.component.html',
  styleUrls: ['./header-nav-links.component.scss'],
  standalone: true,
  imports: [RouterLinkActive, RouterLink],
})
export class HeaderNavLinksComponent {
  navLinks = navigationLinks;
}
