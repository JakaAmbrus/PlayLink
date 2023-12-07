import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FriendRequest } from 'src/app/_models/friends';
import { FriendsService } from 'src/app/_services/friends.service';

@Component({
  selector: 'app-header-notifications',
  templateUrl: './header-notifications.component.html',
  styleUrl: './header-notifications.component.scss',
})
export class HeaderNotificationsComponent {
  @Input() friendRequests: FriendRequest[] = [];

  @Output() requestDeleted: EventEmitter<number> = new EventEmitter();

  constructor(private friendsService: FriendsService) {}

  respondToFriendRequest(friendRequestId: number, accept: boolean): void {
    this.friendsService
      .respondToFriendRequest({
        friendRequestId: friendRequestId,
        accept,
      })
      .subscribe({
        next: (response) => {
          this.requestDeleted.emit(friendRequestId);
          if (response.accepted) {
            console.log(response.friend);
          }
        },
      });
  }

  removeFriendRequest(friendRequestId: number): void {
    this.friendsService.removeFriendRequest(friendRequestId).subscribe({
      next: () => {
        console.log('Friend request deleted');
        this.requestDeleted.emit(friendRequestId);
      },
    });
  }
}
