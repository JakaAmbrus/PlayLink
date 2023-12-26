import { Component, OnInit } from '@angular/core';
import { ProfileUser } from '../../shared/models/users';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { animate, style, transition, trigger } from '@angular/animations';
import { slideInAnimation } from '../../core/animations/route-animations';
import { ProfileNavigationComponent } from './components/profile-navigation/profile-navigation.component';
import { ProfileUserCardComponent } from './components/profile-user-card/profile-user-card.component';
import { AsyncPipe, NgIf } from '@angular/common';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { Observable, catchError, of } from 'rxjs';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';
import { SpinnerComponent } from '../../shared/components/spinner/spinner.component';

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
    SpinnerComponent,
  ],
})
export class ProfileComponent implements OnInit {
  user$?: Observable<ProfileUser>;
  isCurrentUserProfile: boolean = false;
  loadingError: boolean = false;

  constructor(
    public userProfileService: UserProfileService,
    private route: ActivatedRoute,
    private router: Router,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit(): void {
    var username = this.route.snapshot.paramMap.get('username');
    if (username === null) {
      return;
    }

    this.isCurrentUserProfile = this.IsCurrentUser(username);

    this.loadingError = false;
    this.user$ = this.userProfileService.getUser(username).pipe(
      catchError(() => {
        this.loadingError = true;
        setTimeout(() => {
          this.router
            .navigateByUrl('/RefreshComponent', { skipLocationChange: true })
            .then(() => {
              this.router.navigate(['/discover']);
            });
        }, 3000);
        return of();
      })
    );
  }

  prepareRoute(outlet: any) {
    if (outlet.isActivated) {
      const component = outlet.activatedRoute.component;
      if (component === ProfileNavigationComponent) {
        return 'ProfileNavigationComponent';
      } else if (component === ProfileUserCardComponent) {
        return 'ProfileUserCardComponent';
      }
    }
    return (
      outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation
    );
  }

  IsCurrentUser(username: string): boolean {
    const currentUser = this.localStorageService.getItem<string>('username');
    if (currentUser === username) {
      return true;
    }
    return false;
  }
}
