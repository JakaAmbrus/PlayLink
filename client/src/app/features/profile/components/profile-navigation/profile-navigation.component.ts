import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterLinkActive, RouterLink } from '@angular/router';

@Component({
  selector: 'app-profile-navigation',
  templateUrl: './profile-navigation.component.html',
  styleUrl: './profile-navigation.component.scss',
  standalone: true,
  imports: [RouterLinkActive, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileNavigationComponent {
  @Input() username: string = '';
  @Input() isCurrentUserProfile: boolean = false;
}
