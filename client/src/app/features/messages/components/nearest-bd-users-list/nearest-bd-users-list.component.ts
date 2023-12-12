import { Component, OnInit } from '@angular/core';
import { NearestBirthdayUser } from 'src/app/shared/models/users';
import { UsersService } from 'src/app/shared/services/users.service';
import { NearestBdUserDisplayComponent } from './nearest-bd-user-display/nearest-bd-user-display.component';
import { NgIf, NgFor } from '@angular/common';

@Component({
    selector: 'app-nearest-bd-users-list',
    templateUrl: './nearest-bd-users-list.component.html',
    styleUrl: './nearest-bd-users-list.component.scss',
    standalone: true,
    imports: [
        NgIf,
        NgFor,
        NearestBdUserDisplayComponent,
    ],
})
export class NearestBdUsersListComponent implements OnInit {
  nearestBirthdayUsers: NearestBirthdayUser[] = [];
  isLoading: boolean = true;

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.loadNearestBirthdayUsers();
  }

  loadNearestBirthdayUsers(): void {
    this.usersService.getNearestBirthdayUsers().subscribe({
      next: (users) => {
        this.nearestBirthdayUsers = users;
        this.isLoading = false;
        console.log(this.nearestBirthdayUsers);
      },
      error: (err) => {
        console.error('Error loading nearest birthday users', err);
      },
    });
  }
}