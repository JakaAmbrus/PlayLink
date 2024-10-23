import { Component, OnInit } from '@angular/core';

import { NearestBdUserDisplayComponent } from '../nearest-bd-user-display/nearest-bd-user-display.component';
import { NgIf, AsyncPipe } from '@angular/common';
import { NearestBirthdayUser } from '../../models/nearestBirthdayUser';
import { NearestBirthdayService } from '../../services/nearest-birthday.service';
import { Observable, catchError, of } from 'rxjs';

@Component({
  selector: 'app-nearest-bd-users-list',
  templateUrl: './nearest-bd-users-list.component.html',
  styleUrl: './nearest-bd-users-list.component.scss',
  standalone: true,
  imports: [AsyncPipe, NgIf, NearestBdUserDisplayComponent],
})
export class NearestBdUsersListComponent implements OnInit {
  nearestBirthdayUsers$?: Observable<NearestBirthdayUser[]>;
  loadingError: boolean = false;

  constructor(private nearestBirthdayService: NearestBirthdayService) {}

  ngOnInit(): void {
    this.nearestBirthdayUsers$ = this.nearestBirthdayService
      .getNearestBirthdayUsers()
      .pipe(
        catchError(() => {
          this.loadingError = true;
          return of([]);
        })
      );
  }
}
