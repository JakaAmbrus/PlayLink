import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Message } from 'src/app/shared/models/message';
import { LimitTextPipe } from '../../../../shared/pipes/limit-text.pipe';
import { RelativeTimePipe } from '../../../../shared/pipes/relative-time.pipe';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { MessageDisplayService } from '../../services/message-display.service';
import { first } from 'rxjs';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
  selector: 'app-message-display',
  templateUrl: './message-display.component.html',
  styleUrls: ['./message-display.component.scss'],
  standalone: true,
  imports: [
    RouterLink,
    UserAvatarComponent,
    RelativeTimePipe,
    LimitTextPipe,
    SpinnerComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MessageDisplayComponent {
  @Input() message: Message | undefined;

  @Input() container: string | undefined;

  @Output() messageDeleted: EventEmitter<number> = new EventEmitter();

  isLoading: boolean = false;

  constructor(private messageDisplayService: MessageDisplayService) {}

  deleteMessage(id: number): void {
    if (this.isLoading) {
      return;
    }
    this.isLoading = true;

    this.messageDisplayService
      .deleteMessage(id)
      .pipe(first())
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.messageDeleted.emit(id);
        },
        error: () => {
          this.isLoading = false;
        },
      });
  }
}
