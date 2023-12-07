import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from 'src/app/_components/dialog/dialog.component';
import {
  FriendshipStatus,
  FriendshipStatusResponse,
} from 'src/app/_models/friends';
import { ProfileUser } from 'src/app/_models/users';
import { FriendsService } from 'src/app/_services/friends.service';
import { ModeratorService } from 'src/app/_services/moderator.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-profile-user-card',
  templateUrl: './profile-user-card.component.html',
  styleUrl: './profile-user-card.component.scss',
})
export class ProfileUserCardComponent implements OnInit {
  @Input() user: ProfileUser | undefined;

  @Input() isCurrentUserProfile: boolean = false;

  friendshipStatus: string = '';

  constructor(
    private friendsService: FriendsService,
    public dialog: MatDialog,
    public presenceService: PresenceService,
    private moderatorService: ModeratorService,
    private usersService: UsersService
  ) {}

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

  removeFriend(): void {
    if (this.user === undefined) {
      return;
    }
    const dialogRef = this.dialog.open(DialogComponent, {
      data: {
        title: 'Remove Friend',
        message: 'Are you sure you want to remove this friend?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.friendsService.removeFriendship(this.user!.username).subscribe({
          next: () => {
            this.friendshipStatus = 'None';
          },
          error: (err) => {
            console.log(err);
          },
        });
      }
    });
  }

  moderatePicture(): void {
    if (this.user === undefined) {
      return;
    }
    const dialogRef = this.dialog.open(DialogComponent, {
      data: {
        title: 'Moderate Picture',
        message: 'Are you sure you want to remove this picture?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.moderatorService.deleteUserPhoto(this.user!.username).subscribe({
          next: () => {
            this.usersService.invalidateUserCache(this.user!.username);
            this.user!.profilePictureUrl = null;
          },
          error: (err) => {
            console.log(err);
          },
        });
      }
    });
  }
}
