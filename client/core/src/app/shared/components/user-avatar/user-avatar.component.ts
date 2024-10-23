import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RelativeUrlPipe } from '../../pipes/relative-url.pipe';
import { NgOptimizedImage } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrl: './user-avatar.component.scss',
  imports: [RelativeUrlPipe, NgOptimizedImage],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserAvatarComponent {
  @Input() profilePictureUrl: string | null = '';
  @Input() fullName: string = '';
  @Input() gender: string = '';
}
