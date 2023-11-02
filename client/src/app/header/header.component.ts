import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  @Input() theme!: 'theme-light' | 'theme-dark';

  @Output() themeButtonClicked = new EventEmitter<void>();

  handleClick() {
    this.themeButtonClicked.emit();
  }

  isDropdownOpen: boolean = false;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
}
