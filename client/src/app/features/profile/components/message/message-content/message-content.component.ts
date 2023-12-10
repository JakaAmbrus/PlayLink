import { Component, Input } from '@angular/core';
import { Message } from 'src/app/shared/models/messages';

@Component({
  selector: 'app-message-content',
  templateUrl: './message-content.component.html',
  styleUrls: ['./message-content.component.scss'],
})
export class MessageContentComponent {
  @Input() message: Message | undefined;

  @Input() username: string | undefined;

  constructor() {}
}
