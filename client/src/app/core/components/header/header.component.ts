import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FriendsService } from '../../../shared/services/friends.service';
import { FriendRequest } from '../../../shared/models/friends';
import { RouterLink } from '@angular/router';
import { HeaderDropdownComponent } from './components/header-dropdown/header-dropdown.component';
import { HeaderNotificationsComponent } from './components/header-notifications/header-notifications.component';
import { NgIf } from '@angular/common';
import { HeaderNavLinksComponent } from './components/header-nav-links/header-nav-links.component';
import { HeaderLogoComponent } from './components/header-logo/header-logo.component';
import { LocalStorageService } from '../../services/local-storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  standalone: true,
  imports: [
    HeaderLogoComponent,
    HeaderNavLinksComponent,
    NgIf,
    HeaderNotificationsComponent,
    HeaderDropdownComponent,
    RouterLink,
  ],
})
export class HeaderComponent implements OnInit {
  @Input() theme!: 'theme-light' | 'theme-dark';

  @Output() themeButtonClicked = new EventEmitter<void>();

  username: string | null = null;
  isDropdownOpen: boolean = false;
  isNotificationsOpen: boolean = false;
  preventClose: boolean = false;
  isAdmin: boolean = false;
  isModerator: boolean = false;
  friendRequests: FriendRequest[] = [];

  constructor(
    private friendsService: FriendsService,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit(): void {
    this.username = this.localStorageService.getItem<string>('username');
    if (!this.username) {
      return;
    }
    this.checkRoles();
    this.loadNotifications();
  }

  loadNotifications(): void {
    if (!this.username) {
      return;
    }
    this.friendsService.getFriendRequests().subscribe({
      next: (response) => {
        this.friendRequests = response.friendRequests;
      },
    });
  }

  handleClick(): void {
    this.themeButtonClicked.emit();
  }

  toggleDropdown(): void {
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

  toggleNotifications(): void {
    if (!this.username || this.friendRequests.length === 0) {
      return;
    }
    if (this.isNotificationsOpen) {
      this.isNotificationsOpen = false;
    } else {
      this.isNotificationsOpen = true;
    }
  }

  bindClickListener(): void {
    document.addEventListener('click', this.onDocumentClick.bind(this));
  }

  unbindClickListener(): void {
    document.removeEventListener('click', this.onDocumentClick.bind(this));
  }

  onDocumentClick(event: MouseEvent): void {
    if (this.preventClose) {
      return;
    }
    if (this.isDropdownOpen) {
      this.isDropdownOpen = false;
    }
    this.unbindClickListener();
  }

  checkRoles(): void {
    const storedRoles = this.localStorageService.getItem<string>('roles');
    if (!storedRoles) {
      return;
    }
    const roles = storedRoles ? JSON.parse(storedRoles) : [];
    this.isAdmin = roles.includes('Admin');
    this.isModerator = roles.includes('Moderator');
  }

  onRequestDelete(requestId: number): void {
    this.friendRequests = this.friendRequests.filter(
      (fr) => fr.friendRequestId !== requestId
    );
  }
}
