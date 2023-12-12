import { Component, Input } from '@angular/core';
import { SearchUser } from 'src/app/shared/models/users';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-online-user-display',
  templateUrl: './online-user-display.component.html',
  styleUrl: './online-user-display.component.scss',
  standalone: true,
  imports: [NgIf, RouterLink, UserAvatarComponent],
})
export class OnlineUserDisplayComponent {
  @Input() user: SearchUser | undefined;
}
