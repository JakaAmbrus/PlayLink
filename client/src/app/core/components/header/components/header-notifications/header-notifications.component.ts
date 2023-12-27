import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { FriendRequest } from 'src/app/shared/models/friends';
import { FriendsService } from 'src/app/shared/services/friends.service';
import { RelativeTimePipe } from '../../../../../shared/pipes/relative-time.pipe';
import { UserAvatarComponent } from '../../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { first } from 'rxjs';

@Component({
  selector: 'app-header-notifications',
  templateUrl: './header-notifications.component.html',
  styleUrl: './header-notifications.component.scss',
  standalone: true,
  imports: [RouterLink, UserAvatarComponent, RelativeTimePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderNotificationsComponent {
  @Input() friendRequests: FriendRequest[] = [];

  @Output() requestDeleted: EventEmitter<number> = new EventEmitter();

  isLoading: boolean = false;

  constructor(private friendsService: FriendsService) {}

  respondToFriendRequest(friendRequestId: number, accept: boolean): void {
    this.isLoading = true;

    this.friendsService
      .respondToFriendRequest({
        friendRequestId: friendRequestId,
        accept: accept,
      })
      .pipe(first())
      .subscribe({
        next: (response) => {
          this.isLoading = false;
          this.requestDeleted.emit(friendRequestId);
          if (response.requestAccepted) {
            this.friendsService.addFriend(response.friendDto);
          }
        },
        error: () => {
          this.isLoading = false;
        },
      });
  }

  removeFriendRequest(friendRequestId: number): void {
    this.friendsService
      .removeFriendRequest(friendRequestId)
      .pipe(first())
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.requestDeleted.emit(friendRequestId);
        },
        error: () => {
          this.isLoading = false;
        },
      });
  }
}
