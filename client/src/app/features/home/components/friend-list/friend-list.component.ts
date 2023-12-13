import { Component, OnInit } from '@angular/core';
import { Friend } from 'src/app/shared/models/friends';
import { FriendsService } from 'src/app/shared/services/friends.service';
import { FriendDisplayComponent } from '../friend-display/friend-display.component';
import { RouterLink } from '@angular/router';
import { NgIf, NgFor, AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrl: './friend-list.component.scss',
  standalone: true,
  imports: [AsyncPipe, NgIf, RouterLink, NgFor, FriendDisplayComponent],
})
export class FriendListComponent implements OnInit {
  friends$ = this.friendsService.friends$;

  constructor(private friendsService: FriendsService) {}

  ngOnInit(): void {
    this.friendsService.getFriends().subscribe();
  }
}
