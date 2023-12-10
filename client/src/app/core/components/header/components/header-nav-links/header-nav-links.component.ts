import { Component } from '@angular/core';
import { navigationLinks } from '../../nav-links';

@Component({
  selector: 'app-header-nav-links',
  templateUrl: './header-nav-links.component.html',
  styleUrls: ['./header-nav-links.component.scss'],
})
export class HeaderNavLinksComponent {
  navLinks = navigationLinks;
}
