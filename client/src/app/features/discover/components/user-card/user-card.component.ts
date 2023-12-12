import { Component, Input } from '@angular/core';
import { PresenceService } from 'src/app/shared/services/presence.service';
import { FirstWordPipe } from '../../../../shared/pipes/first-word.pipe';
import { RelativeUrlPipe } from '../../../../shared/pipes/relative-url.pipe';
import { RouterLink } from '@angular/router';
import { NgIf, NgClass, NgOptimizedImage, AsyncPipe } from '@angular/common';
import { User } from '../../models/discoverUser';

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
export class UserCardComponent {
  @Input() user: User | undefined;

  isHovered: boolean = false;

  onHover(hovering: boolean): void {
    this.isHovered = hovering;
  }

  constructor(public presenceService: PresenceService) {}
}
