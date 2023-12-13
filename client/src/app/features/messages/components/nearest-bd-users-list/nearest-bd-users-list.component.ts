import { Component, OnInit } from '@angular/core';

import { NearestBdUserDisplayComponent } from '../nearest-bd-user-display/nearest-bd-user-display.component';
import { NgIf, NgFor, AsyncPipe } from '@angular/common';
import { NearestBirthdayUser } from '../../models/nearestBirthdayUser';
import { NearestBirthdayService } from '../../services/nearest-birthday.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-nearest-bd-users-list',
  templateUrl: './nearest-bd-users-list.component.html',
  styleUrl: './nearest-bd-users-list.component.scss',
  standalone: true,
  imports: [AsyncPipe, NgIf, NgFor, NearestBdUserDisplayComponent],
})
export class NearestBdUsersListComponent implements OnInit {
  nearestBirthdayUsers$?: Observable<NearestBirthdayUser[]>;

  constructor(private nearestBirthdayService: NearestBirthdayService) {}

  ngOnInit(): void {
    this.nearestBirthdayUsers$ =
      this.nearestBirthdayService.getNearestBirthdayUsers();
  }
}
