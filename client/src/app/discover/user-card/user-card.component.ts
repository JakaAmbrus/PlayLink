import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/_models/users';

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

  constructor() {}

  ngOnInit(): void {
    this.username = localStorage.getItem('user');
  }
}
