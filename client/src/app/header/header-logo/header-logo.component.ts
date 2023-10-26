import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-header-logo',
  templateUrl: './header-logo.component.html',
  styleUrls: ['./header-logo.component.scss'],
})
export class HeaderLogoComponent {
  @Input() theme!: 'theme-light' | 'theme-dark';

  lightImagePath: string = 'assets/images/header/logo-grey.png';
  darkImagePath: string = 'assets/images/header/logo-white.png';

  isHovered: boolean = false;

  onHover(hovering: boolean): void {
    this.isHovered = hovering;
  }
}
