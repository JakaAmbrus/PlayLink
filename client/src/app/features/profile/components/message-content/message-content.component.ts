import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Message } from 'src/app/shared/models/message';
import { TimeagoModule } from 'ngx-timeago';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { NgIf, NgClass } from '@angular/common';

@Component({
  selector: 'app-message-content',
  templateUrl: './message-content.component.html',
  styleUrls: ['./message-content.component.scss'],
  standalone: true,
  imports: [NgIf, NgClass, UserAvatarComponent, TimeagoModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MessageContentComponent {
  @Input() message: Message | undefined;

  @Input() username: string | undefined;
}
