import { Component, OnInit } from '@angular/core';
import { Observable, combineLatest, map } from 'rxjs';
import { SearchUser } from 'src/app/shared/models/users';
import { PresenceService } from 'src/app/shared/services/presence.service';
import { OnlineUserDisplayComponent } from '../online-user-display/online-user-display.component';
import { NgIf, NgFor, AsyncPipe } from '@angular/common';
import { UserSearchService } from 'src/app/shared/services/user-search.service';

@Component({
  selector: 'app-online-users-list',
  templateUrl: './online-users-list.component.html',
  styleUrl: './online-users-list.component.scss',
  standalone: true,
  imports: [AsyncPipe, NgIf, NgFor, OnlineUserDisplayComponent],
})
export class OnlineUsersListComponent implements OnInit {
  onlineUsers$?: Observable<SearchUser[]>;

  constructor(
    public userSearchService: UserSearchService,
    private presenceService: PresenceService
  ) {}

  ngOnInit(): void {
    this.onlineUsers$ = combineLatest([
      this.userSearchService.getSearchUsers(),
      this.presenceService.onlineUsers$,
    ]).pipe(
      map(([users, onlineUserIds]) =>
        users.filter((user) => onlineUserIds.includes(user.appUserId))
      )
    );
  }
}
