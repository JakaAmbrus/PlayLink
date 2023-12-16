import { Component, Input } from '@angular/core';
import { Friend } from 'src/app/shared/models/friends';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf, DatePipe } from '@angular/common';

@Component({
    selector: 'app-friend-display',
    templateUrl: './friend-display.component.html',
    styleUrl: './friend-display.component.scss',
    standalone: true,
    imports: [
        NgIf,
        RouterLink,
        UserAvatarComponent,
        DatePipe,
    ],
})
export class FriendDisplayComponent {
  @Input() friend: Friend | undefined;
}