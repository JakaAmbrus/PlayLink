import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  @Input() theme!: 'theme-light' | 'theme-dark';

  @Output() themeButtonClicked = new EventEmitter<void>();

  username: string | null = null;

  handleClick() {
    this.themeButtonClicked.emit();
  }

  isDropdownOpen: boolean = false;
  preventClose: boolean = false;

  toggleDropdown() {
    this.username = localStorage.getItem('user');

    if (this.isDropdownOpen) {
      this.isDropdownOpen = false;
      this.unbindClickListener();
    } else {
      this.preventClose = true;
      this.isDropdownOpen = true;
      setTimeout(() => {
        this.bindClickListener();
        this.preventClose = false;
      });
    }
  }

  bindClickListener() {
    document.addEventListener('click', this.onDocumentClick.bind(this));
  }

  unbindClickListener() {
    document.removeEventListener('click', this.onDocumentClick.bind(this));
  }

  onDocumentClick(event: MouseEvent) {
    if (this.preventClose) {
      return;
    }
    this.isDropdownOpen = false;
    this.unbindClickListener();
  }
}
