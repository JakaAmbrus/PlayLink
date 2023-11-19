import { Component, OnInit } from '@angular/core';
import { ProfileUser } from '../_models/users';
import { UsersService } from '../_services/users.service';
import { ActivatedRoute } from '@angular/router';
import { animate, style, transition, trigger } from '@angular/animations';

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
  ],
})
export class ProfileComponent implements OnInit {
  user: ProfileUser | undefined;
  dateOfBirth: string | undefined;
  isCurrentUserProfile: boolean = false;

  constructor(
    private usersService: UsersService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    var username = this.route.snapshot.paramMap.get('username');
    console.log(username);

    if (username === null) {
      return;
    }

    this.usersService.getUser(username).subscribe({
      next: (user) => {
        this.user = user;
        this.isCurrentUserProfile = this.IsCurrentUser(this.user.username);
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
