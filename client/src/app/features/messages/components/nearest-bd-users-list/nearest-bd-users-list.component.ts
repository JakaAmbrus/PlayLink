import { Component, OnInit } from '@angular/core';

import { NearestBdUserDisplayComponent } from '../nearest-bd-user-display/nearest-bd-user-display.component';
import { NgIf, NgFor } from '@angular/common';
import { NearestBirthdayUser } from '../../models/nearestBirthdayUser';
import { NearestBirthdayService } from '../../services/nearest-birthday.service';

@Component({
  selector: 'app-nearest-bd-users-list',
  templateUrl: './nearest-bd-users-list.component.html',
  styleUrl: './nearest-bd-users-list.component.scss',
  standalone: true,
  imports: [NgIf, NgFor, NearestBdUserDisplayComponent],
})
export class NearestBdUsersListComponent implements OnInit {
  nearestBirthdayUsers: NearestBirthdayUser[] = [];
  isLoading: boolean = true;

  constructor(private nearestBirthdayService: NearestBirthdayService) {}

  ngOnInit(): void {
    this.loadNearestBirthdayUsers();
  }

  loadNearestBirthdayUsers(): void {
    this.nearestBirthdayService.getNearestBirthdayUsers().subscribe({
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
