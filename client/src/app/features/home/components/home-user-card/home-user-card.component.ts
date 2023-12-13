import { Component, OnInit } from '@angular/core';
import { ProfileUser } from 'src/app/shared/models/users';
import { RelativeTimePipe } from '../../../../shared/pipes/relative-time.pipe';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf, DatePipe, AsyncPipe } from '@angular/common';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { Observable } from 'rxjs';

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

  constructor(public userProfileService: UserProfileService) {}

  ngOnInit(): void {
    this.username = localStorage.getItem('user');

    if (this.username === null) {
      return;
    }

    this.user$ = this.userProfileService.getUser(this.username);
  }
}
