import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { FriendsService } from '../../../shared/services/friends.service';
import { FriendRequest } from '../../../shared/models/friends';
import { RouterLink } from '@angular/router';
import { HeaderDropdownComponent } from './components/header-dropdown/header-dropdown.component';
import { HeaderNotificationsComponent } from './components/header-notifications/header-notifications.component';
import { HeaderNavLinksComponent } from './components/header-nav-links/header-nav-links.component';
import { HeaderLogoComponent } from './components/header-logo/header-logo.component';
import { LocalStorageService } from '../../services/local-storage.service';
import {
  Subscription,
  concatMap,
  first,
  interval,
  startWith,
  take,
} from 'rxjs';
import { ClickOutsideService } from 'src/app/shared/services/click-outside.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  standalone: true,
  imports: [
    HeaderLogoComponent,
    HeaderNavLinksComponent,
    HeaderNotificationsComponent,
    HeaderDropdownComponent,
    RouterLink,
  ],
})
export class HeaderComponent implements OnInit, OnDestroy {
  @Input() theme!: 'theme-light' | 'theme-dark';

  @Output() themeButtonClicked = new EventEmitter<void>();

  username: string | null = null;
  isDropdownOpen: boolean = false;
  isNotificationsOpen: boolean = false;
  preventClose: boolean = false;
  isAdmin: boolean = false;
  isModerator: boolean = false;
  friendRequests: FriendRequest[] = [];
  private subscription = new Subscription();

  constructor(
    private friendsService: FriendsService,
    private localStorageService: LocalStorageService,
    private clickOutsideService: ClickOutsideService
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
    this.subscription.add(
      interval(30000)
        .pipe(
          startWith(0),
          take(15),
          concatMap(() => this.friendsService.getFriendRequests().pipe(first()))
        )
        .subscribe({
          next: (response) => {
            this.friendRequests = response.friendRequests;
          },
        })
    );
  }

  onRequestDelete(requestId: number): void {
    this.friendRequests = this.friendRequests.filter(
      (fr) => fr.friendRequestId !== requestId
    );
  }

  handleClick(): void {
    this.themeButtonClicked.emit();
  }

  toggleDropdown(): void {
    if (!this.username) return;

    this.isDropdownOpen = !this.isDropdownOpen;
    if (this.isDropdownOpen) {
      this.clickOutsideService.bind(this, () => {
        this.isDropdownOpen = false;
      });
    } else {
      this.clickOutsideService.unbind(this);
    }
  }

  toggleNotifications(): void {
    if (!this.username || this.friendRequests.length === 0) {
      return;
    }
    this.isNotificationsOpen = !this.isNotificationsOpen;
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

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
