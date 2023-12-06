import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  @Input() theme!: 'theme-light' | 'theme-dark';

  @Output() themeButtonClicked = new EventEmitter<void>();

  username: string | null = null;
  isDropdownOpen: boolean = false;
  preventClose: boolean = false;
  isAdmin: boolean = false;

  ngOnInit() {
    this.checkRoleAdmin();
  }

  handleClick() {
    this.themeButtonClicked.emit();
  }

  toggleDropdown() {
    this.username = localStorage.getItem('user');

    if (!this.username) {
      return;
    }

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

  checkRoleAdmin(): void {
    const storedRoles = localStorage.getItem('roles');
    if (!storedRoles) {
      return;
    }
    const roles = storedRoles ? JSON.parse(storedRoles) : [];
    this.isAdmin = roles.includes('Admin');
  }
}
