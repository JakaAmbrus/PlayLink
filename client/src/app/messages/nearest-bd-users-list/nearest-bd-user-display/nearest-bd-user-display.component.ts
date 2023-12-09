import { Component, Input } from '@angular/core';
import { NearestBirthdayUser } from 'src/app/_models/users';

@Component({
  selector: 'app-nearest-bd-user-display',
  templateUrl: './nearest-bd-user-display.component.html',
  styleUrl: './nearest-bd-user-display.component.scss',
})
export class NearestBdUserDisplayComponent {
  @Input() user: NearestBirthdayUser | undefined;
}
