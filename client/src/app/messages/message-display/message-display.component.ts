import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Message } from 'src/app/_models/messages';

@Component({
  selector: 'app-message-display',
  templateUrl: './message-display.component.html',
  styleUrls: ['./message-display.component.scss'],
})
export class MessageDisplayComponent {
  @Input() message: Message | undefined;

  @Input() container: string | undefined;

  @Output() messageDeleted: EventEmitter<number> = new EventEmitter();

  deleteMessage(id: number): void {
    this.messageDeleted.emit(id);
  }
}
