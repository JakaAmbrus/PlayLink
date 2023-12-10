import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-profile-navigation',
  templateUrl: './profile-navigation.component.html',
  styleUrl: './profile-navigation.component.scss',
})
export class ProfileNavigationComponent {
  @Input() username: string = '';
  @Input() isCurrentUserProfile: boolean = false;
}
