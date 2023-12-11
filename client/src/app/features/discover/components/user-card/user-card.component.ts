import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/shared/models/users';
import { PresenceService } from 'src/app/shared/services/presence.service';
import { FirstWordPipe } from '../../../../shared/pipes/first-word.pipe';
import { RelativeUrlPipe } from '../../../../shared/pipes/relative-url.pipe';
import { RouterLink } from '@angular/router';
import { NgIf, NgClass, NgOptimizedImage, AsyncPipe } from '@angular/common';

@Component({
    selector: 'app-user-card',
    templateUrl: './user-card.component.html',
    styleUrls: ['./user-card.component.scss'],
    standalone: true,
    imports: [
        NgIf,
        NgClass,
        RouterLink,
        NgOptimizedImage,
        RelativeUrlPipe,
        AsyncPipe,
        FirstWordPipe,
    ],
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
