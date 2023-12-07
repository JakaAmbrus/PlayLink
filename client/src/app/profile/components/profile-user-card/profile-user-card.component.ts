import { Component, Input, OnInit } from '@angular/core';
import {
  FriendshipStatus,
  FriendshipStatusResponse,
} from 'src/app/_models/friends';
import { ProfileUser } from 'src/app/_models/users';
import { FriendsService } from 'src/app/_services/friends.service';

@Component({
  selector: 'app-profile-user-card',
  templateUrl: './profile-user-card.component.html',
  styleUrl: './profile-user-card.component.scss',
})
export class ProfileUserCardComponent implements OnInit {
  @Input() user: ProfileUser | undefined;

  @Input() isCurrentUserProfile: boolean = false;

  friendshipStatus: string = '';

  constructor(private friendsService: FriendsService) {}

  ngOnInit(): void {
    this.loadFriendStatus();
  }

  loadFriendStatus(): void {
    if (this.user === undefined) {
      return;
    }

    this.friendsService.getFriendRequestStatus(this.user.username).subscribe({
      next: (response: FriendshipStatusResponse) => {
        this.friendshipStatus = FriendshipStatus[response.status];
        console.log(this.friendshipStatus);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  addFriend(): void {
    if (this.user === undefined) {
      return;
    }

    this.friendsService.sendFriendRequest(this.user.username).subscribe({
      next: () => {
        this.friendshipStatus = 'Pending';
        console.log('Friend request sent');
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
