import { Component, Input } from '@angular/core';
import { NgIf } from '@angular/common';
import { RouterLinkActive, RouterLink } from '@angular/router';

@Component({
    selector: 'app-profile-navigation',
    templateUrl: './profile-navigation.component.html',
    styleUrl: './profile-navigation.component.scss',
    standalone: true,
    imports: [
        RouterLinkActive,
        RouterLink,
        NgIf,
    ],
})
export class ProfileNavigationComponent {
  @Input() username: string = '';
  @Input() isCurrentUserProfile: boolean = false;
}
