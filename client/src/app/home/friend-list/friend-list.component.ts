import { Component, OnInit } from '@angular/core';
import { Friend } from 'src/app/_models/friends';
import { FriendsService } from 'src/app/_services/friends.service';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrl: './friend-list.component.scss',
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
