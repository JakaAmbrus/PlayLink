import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/shared/models/users';
import { PresenceService } from 'src/app/shared/services/presence.service';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss'],
})
export class UserCardComponent implements OnInit {
  @Input() user: User | undefined;

  username: string | null = null;

  isHovered: boolean = false;

  onHover(hovering: boolean): void {
    this.isHovered = hovering;
  }

  constructor(public presenceService: PresenceService) {}

  ngOnInit(): void {
    this.username = localStorage.getItem('user');
  }
}
