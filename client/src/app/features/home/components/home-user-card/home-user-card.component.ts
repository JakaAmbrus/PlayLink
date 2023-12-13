import { Component, OnInit } from '@angular/core';
import { ProfileUser } from 'src/app/shared/models/users';
import { RelativeTimePipe } from '../../../../shared/pipes/relative-time.pipe';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf, DatePipe } from '@angular/common';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';

@Component({
  selector: 'app-home-user-card',
  templateUrl: './home-user-card.component.html',
  styleUrl: './home-user-card.component.scss',
  standalone: true,
  imports: [NgIf, RouterLink, UserAvatarComponent, RelativeTimePipe, DatePipe],
})
export class HomeUserCardComponent implements OnInit {
  username: string | null = null;
  user: ProfileUser | undefined;
  isLoading: boolean = true;

  constructor(private userProfileService: UserProfileService) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    this.username = localStorage.getItem('user');

    if (this.username === null) {
      return;
    }

    this.userProfileService.getUser(this.username).subscribe({
      next: (user) => {
        this.user = user;
        this.isLoading = false;
      },
    });
  }
}
