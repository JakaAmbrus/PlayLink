import { Component, OnInit } from '@angular/core';
import { combineLatest, map } from 'rxjs';
import { SearchUser } from 'src/app/shared/models/users';
import { PresenceService } from 'src/app/shared/services/presence.service';
import { OnlineUserDisplayComponent } from '../online-user-display/online-user-display.component';
import { NgIf, NgFor } from '@angular/common';
import { UserSearchService } from 'src/app/shared/services/user-search.service';

@Component({
  selector: 'app-online-users-list',
  templateUrl: './online-users-list.component.html',
  styleUrl: './online-users-list.component.scss',
  standalone: true,
  imports: [NgIf, NgFor, OnlineUserDisplayComponent],
})
export class OnlineUsersListComponent implements OnInit {
  onlineUsers: SearchUser[] = [];

  constructor(
    private userSearchService: UserSearchService,
    private presenceService: PresenceService
  ) {}

  ngOnInit(): void {
    this.loadOnlineUsers();
  }

  loadOnlineUsers(): void {
    combineLatest([
      this.userSearchService.getSearchUsers(),
      this.presenceService.onlineUsers$,
    ])
      .pipe(
        map(([users, onlineUserIds]) => {
          return users.filter((user) => onlineUserIds.includes(user.appUserId));
        })
      )
      .subscribe({
        next: (filteredUsers) => {
          this.onlineUsers = filteredUsers;
          console.log(this.onlineUsers);
        },
        error: (err) => {
          console.error('Error loading online users', err);
        },
      });
  }
}
