import { Component, Input } from '@angular/core';
import { LikedUser } from '../../models/likedUser';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-liked-users-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './liked-users-list.component.html',
  styleUrl: './liked-users-list.component.scss',
})
export class LikedUsersListComponent {
  @Input() likedUsers: LikedUser[] | undefined;
}
