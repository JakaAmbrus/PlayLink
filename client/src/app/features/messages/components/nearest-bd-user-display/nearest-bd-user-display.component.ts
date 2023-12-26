import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf, DatePipe } from '@angular/common';
import { NearestBirthdayUser } from '../../models/nearestBirthdayUser';

@Component({
  selector: 'app-nearest-bd-user-display',
  templateUrl: './nearest-bd-user-display.component.html',
  styleUrl: './nearest-bd-user-display.component.scss',
  standalone: true,
  imports: [NgIf, RouterLink, UserAvatarComponent, DatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NearestBdUserDisplayComponent {
  @Input() user: NearestBirthdayUser | undefined;
}
