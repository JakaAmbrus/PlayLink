import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from 'src/app/shared/components/dialog/dialog.component';
import {
  FriendshipStatus,
  FriendshipStatusResponse,
} from 'src/app/shared/models/friends';
import { ProfileUser } from 'src/app/shared/models/users';
import { FriendsService } from 'src/app/shared/services/friends.service';
import { ModeratorService } from 'src/app/shared/services/moderator.service';
import { PresenceService } from 'src/app/shared/services/presence.service';
import { RelativeTimePipe } from '../../../../shared/pipes/relative-time.pipe';
import { RelativeUrlPipe } from '../../../../shared/pipes/relative-url.pipe';
import { RouterLink } from '@angular/router';
import {
  NgIf,
  NgOptimizedImage,
  NgSwitch,
  NgSwitchCase,
  AsyncPipe,
  DatePipe,
} from '@angular/common';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-profile-user-card',
  templateUrl: './profile-user-card.component.html',
  styleUrl: './profile-user-card.component.scss',
  standalone: true,
  imports: [
    NgIf,
    NgOptimizedImage,
    NgSwitch,
    NgSwitchCase,
    RouterLink,
    RelativeUrlPipe,
    RelativeTimePipe,
    AsyncPipe,
    DatePipe,
  ],
})
export class ProfileUserCardComponent implements OnInit, OnDestroy {
  @Input() user: ProfileUser | undefined;

  @Input() isCurrentUserProfile: boolean = false;

  private destroy$ = new Subject<void>();
  friendshipStatus: string = '';

  constructor(
    public dialog: MatDialog,
    public presenceService: PresenceService,
    private moderatorService: ModeratorService,
    private userProfileService: UserProfileService,
    private friendsService: FriendsService
  ) {}

  ngOnInit(): void {
    if (this.isCurrentUserProfile) {
      this.friendshipStatus = 'Current';
      return;
    }
    this.loadFriendStatus();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadFriendStatus(): void {
    if (this.user === undefined) {
      return;
    }

    this.friendsService
      .getFriendRequestStatus(this.user.username)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
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

    this.friendsService
      .sendFriendRequest(this.user.username)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
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
            this.userProfileService.invalidateUserCache(this.user!.username);
            this.user!.profilePictureUrl = null;
          },
          error: (err) => {
            console.log(err);
          },
        });
      }
    });
  }

  moderateDescription(): void {
    if (this.user === undefined) {
      return;
    }
    const dialogRef = this.dialog.open(DialogComponent, {
      data: {
        title: 'Moderate Description',
        message: 'Are you sure you want to remove this description?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.moderatorService
          .deleteUserDescription(this.user!.username)
          .subscribe({
            next: () => {
              this.userProfileService.invalidateUserCache(this.user!.username);
              this.user!.description = null;
            },
            error: (err) => {
              console.log(err);
            },
          });
      }
    });
  }
}
