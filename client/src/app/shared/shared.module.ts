import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserAvatarComponent } from './components/user-avatar/user-avatar.component';
import { RelativeUrlPipe } from './pipes/relative-url.pipe';
import { NgOptimizedImage } from '@angular/common';

@NgModule({
  declarations: [UserAvatarComponent],
  exports: [UserAvatarComponent],
  imports: [CommonModule, NgOptimizedImage, RelativeUrlPipe],
})
export class SharedModule {}
