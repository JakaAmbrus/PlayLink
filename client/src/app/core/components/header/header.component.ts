import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FriendsService } from '../../../shared/services/friends.service';
import { FriendRequest } from '../../../shared/models/friends';

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
  isNotificationsOpen: boolean = false;
  preventClose: boolean = false;
  isAdmin: boolean = false;
  isModerator: boolean = false;
  friendRequests: FriendRequest[] = [];

  constructor(private friendsService: FriendsService) {}

  ngOnInit(): void {
    this.username = localStorage.getItem('user');
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
        console.log(this.friendRequests);
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
    if (this.isDropdownOpen) this.isDropdownOpen = false;
    if (this.isNotificationsOpen) this.isNotificationsOpen = false;
    this.unbindClickListener();
  }

  checkRoles(): void {
    const storedRoles = localStorage.getItem('roles');
    if (!storedRoles) {
      return;
    }
    const roles = storedRoles ? JSON.parse(storedRoles) : [];
    this.isAdmin = roles.includes('Admin');
    this.isModerator = roles.includes('Moderator');
  }

  onRequestDelete(requestId: number): void {
    console.log('Friend request deleted 2');
    this.friendRequests = this.friendRequests.filter(
      (fr) => fr.friendRequestId !== requestId
    );
    console.log(this.friendRequests);
  }
}
