import { Component, OnInit } from '@angular/core';
import { ProfileUser } from '../../shared/models/users';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { animate, style, transition, trigger } from '@angular/animations';
import { slideInAnimation } from '../../route-animations';
import { ProfileNavigationComponent } from './components/profile-navigation/profile-navigation.component';
import { ProfileUserCardComponent } from './components/profile-user-card/profile-user-card.component';
import { NgIf } from '@angular/common';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  animations: [
    trigger('ProfileAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'scale(0.5)' }),
        animate('500ms ease', style({ opacity: 1, transform: 'scale(1)' })),
      ]),
    ]),
    slideInAnimation,
  ],
  standalone: true,
  imports: [
    NgIf,
    ProfileUserCardComponent,
    ProfileNavigationComponent,
    RouterOutlet,
  ],
})
export class ProfileComponent implements OnInit {
  user: ProfileUser | undefined;
  dateOfBirth: string | undefined;
  isCurrentUserProfile: boolean = false;

  constructor(
    private userProfileService: UserProfileService,
    private route: ActivatedRoute
  ) {}

  prepareRoute(outlet: any) {
    return (
      outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation
    );
  }

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    var username = this.route.snapshot.paramMap.get('username');
    if (username === null) {
      return;
    }

    this.isCurrentUserProfile = this.IsCurrentUser(username);

    this.userProfileService.getUser(username).subscribe({
      next: (user) => {
        console.log(user);

        this.user = user;
      },
    });
  }

  IsCurrentUser(username: string): boolean {
    const currentUser = localStorage.getItem('user');
    if (currentUser === username) {
      return true;
    }
    return false;
  }
}
