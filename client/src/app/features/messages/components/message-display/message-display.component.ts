import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Message } from 'src/app/shared/models/messages';
import { LimitTextPipe } from '../../../../shared/pipes/limit-text.pipe';
import { RelativeTimePipe } from '../../../../shared/pipes/relative-time.pipe';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-message-display',
    templateUrl: './message-display.component.html',
    styleUrls: ['./message-display.component.scss'],
    standalone: true,
    imports: [
        NgIf,
        RouterLink,
        UserAvatarComponent,
        RelativeTimePipe,
        LimitTextPipe,
    ],
})
export class MessageDisplayComponent {
  @Input() message: Message | undefined;

  @Input() container: string | undefined;

  @Output() messageDeleted: EventEmitter<number> = new EventEmitter();

  deleteMessage(id: number): void {
    this.messageDeleted.emit(id);
  }
}
