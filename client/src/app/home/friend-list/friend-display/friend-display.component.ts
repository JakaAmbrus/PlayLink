import { Component, Input } from '@angular/core';
import { Friend } from 'src/app/_models/friends';

@Component({
  selector: 'app-friend-display',
  templateUrl: './friend-display.component.html',
  styleUrl: './friend-display.component.scss',
})
export class FriendDisplayComponent {
  @Input() friend: Friend | undefined;
}
