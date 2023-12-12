import { Component, OnInit } from '@angular/core';
import { Friend } from 'src/app/shared/models/friends';
import { FriendsService } from 'src/app/shared/services/friends.service';
import { FriendDisplayComponent } from '../friend-display/friend-display.component';
import { RouterLink } from '@angular/router';
import { NgIf, NgFor } from '@angular/common';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrl: './friend-list.component.scss',
  standalone: true,
  imports: [NgIf, RouterLink, NgFor, FriendDisplayComponent],
})
export class FriendListComponent implements OnInit {
  friends: Friend[] = [];

  constructor(private friendsService: FriendsService) {}

  ngOnInit(): void {
    this.friendsService.getFriends().subscribe();
    this.friendsUpdates();
  }

  friendsUpdates() {
    this.friendsService.friends$.subscribe((friends) => {
      this.friends = friends;
    });
  }
}
