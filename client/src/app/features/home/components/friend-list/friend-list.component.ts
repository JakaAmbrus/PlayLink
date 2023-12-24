import { Component, OnInit } from '@angular/core';
import { Friend } from 'src/app/shared/models/friends';
import { FriendsService } from 'src/app/shared/services/friends.service';
import { FriendDisplayComponent } from '../friend-display/friend-display.component';
import { RouterLink } from '@angular/router';
import { NgIf, NgFor, AsyncPipe } from '@angular/common';
import { Observable, catchError, of } from 'rxjs';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrl: './friend-list.component.scss',
  standalone: true,
  imports: [AsyncPipe, NgIf, RouterLink, NgFor, FriendDisplayComponent],
})
export class FriendListComponent implements OnInit {
  friends$?: Observable<Friend[]>;
  loadingError: boolean = false;

  constructor(private friendsService: FriendsService) {}

  ngOnInit(): void {
    this.loadingError = false;

    this.friends$ = this.friendsService.getFriends().pipe(
      catchError(() => {
        this.loadingError = true;
        return of([]);
      })
    );
  }
}
