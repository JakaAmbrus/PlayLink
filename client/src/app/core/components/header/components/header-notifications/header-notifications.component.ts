import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FriendRequest } from 'src/app/shared/models/friends';
import { FriendsService } from 'src/app/shared/services/friends.service';
import { RelativeTimePipe } from '../../../../../shared/pipes/relative-time.pipe';
import { UserAvatarComponent } from '../../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';

@Component({
    selector: 'app-header-notifications',
    templateUrl: './header-notifications.component.html',
    styleUrl: './header-notifications.component.scss',
    standalone: true,
    imports: [
        NgFor,
        NgIf,
        RouterLink,
        UserAvatarComponent,
        RelativeTimePipe,
    ],
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
          console.log(response);
          this.requestDeleted.emit(friendRequestId);
          if (response.requestAccepted) {
            this.friendsService.addFriend(response.friendDto);
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
