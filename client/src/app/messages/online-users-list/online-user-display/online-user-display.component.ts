import { Component, Input } from '@angular/core';
import { SearchUser } from 'src/app/_models/users';

@Component({
  selector: 'app-online-user-display',
  templateUrl: './online-user-display.component.html',
  styleUrl: './online-user-display.component.scss',
})
export class OnlineUserDisplayComponent {
  @Input() user: SearchUser | undefined;
}
