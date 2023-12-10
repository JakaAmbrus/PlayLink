import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrl: './user-avatar.component.scss',
})
export class UserAvatarComponent {
  @Input() profilePictureUrl: string | null = '';
  @Input() fullName: string = '';
  @Input() gender: string = '';
}
