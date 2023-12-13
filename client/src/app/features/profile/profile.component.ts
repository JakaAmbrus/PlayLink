import { Component, OnInit } from '@angular/core';
import { ProfileUser } from '../../shared/models/users';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { animate, style, transition, trigger } from '@angular/animations';
import { slideInAnimation } from '../../route-animations';
import { ProfileNavigationComponent } from './components/profile-navigation/profile-navigation.component';
import { ProfileUserCardComponent } from './components/profile-user-card/profile-user-card.component';
import { AsyncPipe, NgIf } from '@angular/common';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { Observable } from 'rxjs';

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
    AsyncPipe,
    NgIf,
    ProfileUserCardComponent,
    ProfileNavigationComponent,
    RouterOutlet,
  ],
})
export class ProfileComponent implements OnInit {
  user$?: Observable<ProfileUser>;
  isCurrentUserProfile: boolean = false;

  constructor(
    public userProfileService: UserProfileService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    var username = this.route.snapshot.paramMap.get('username');
    if (username === null) {
      return;
    }

    this.isCurrentUserProfile = this.IsCurrentUser(username);

    this.user$ = this.userProfileService.getUser(username);
  }

  prepareRoute(outlet: any) {
    return (
      outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation
    );
  }

  IsCurrentUser(username: string): boolean {
    const currentUser = localStorage.getItem('user');
    if (currentUser === username) {
      return true;
    }
    return false;
  }
}
