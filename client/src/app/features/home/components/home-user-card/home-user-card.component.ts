import { Component, OnInit } from '@angular/core';
import { ProfileUser } from 'src/app/shared/models/users';
import { RelativeTimePipe } from '../../../../shared/pipes/relative-time.pipe';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf, DatePipe, AsyncPipe } from '@angular/common';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { Observable, catchError, of } from 'rxjs';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';

@Component({
  selector: 'app-home-user-card',
  templateUrl: './home-user-card.component.html',
  styleUrl: './home-user-card.component.scss',
  standalone: true,
  imports: [
    AsyncPipe,
    NgIf,
    RouterLink,
    UserAvatarComponent,
    RelativeTimePipe,
    DatePipe,
  ],
})
export class HomeUserCardComponent implements OnInit {
  user$?: Observable<ProfileUser>;
  username: string | null = null;
  loadingError: boolean = false;

  constructor(
    public userProfileService: UserProfileService,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit(): void {
    this.username = this.localStorageService.getItem('username');

    if (this.username === null) {
      return;
    }
    this.loadingError = false;

    this.user$ = this.userProfileService.getUser(this.username).pipe(
      catchError(() => {
        this.loadingError = true;
        return of();
      })
    );
  }
}
