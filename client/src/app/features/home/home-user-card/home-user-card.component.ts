import { Component, OnInit } from '@angular/core';
import { ProfileUser } from 'src/app/shared/models/users';
import { UsersService } from 'src/app/shared/services/users.service';

@Component({
  selector: 'app-home-user-card',
  templateUrl: './home-user-card.component.html',
  styleUrl: './home-user-card.component.scss',
})
export class HomeUserCardComponent implements OnInit {
  username: string | null = null;
  user: ProfileUser | undefined;
  isLoading: boolean = true;

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    this.username = localStorage.getItem('user');

    if (this.username === null) {
      return;
    }

    this.usersService.getUser(this.username).subscribe({
      next: (user) => {
        this.user = user;
        this.isLoading = false;
      },
    });
  }
}
