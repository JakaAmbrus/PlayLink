import { Component, OnInit } from '@angular/core';
import { combineLatest, map } from 'rxjs';
import { SearchUser } from 'src/app/_models/users';
import { PresenceService } from 'src/app/_services/presence.service';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-online-users-list',
  templateUrl: './online-users-list.component.html',
  styleUrl: './online-users-list.component.scss',
})
export class OnlineUsersListComponent implements OnInit {
  onlineUsers: SearchUser[] = [];

  constructor(
    private usersService: UsersService,
    private presenceService: PresenceService
  ) {}

  ngOnInit(): void {
    this.loadOnlineUsers();
  }

  loadOnlineUsers(): void {
    combineLatest([
      this.usersService.getSearchUsers(),
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
