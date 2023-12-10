import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserAvatarComponent } from './components/user-avatar/user-avatar.component';
import { RelativeUrlPipe } from './pipes/relative-url.pipe';
import { NgOptimizedImage } from '@angular/common';

@NgModule({
  declarations: [],
  exports: [
    CommonModule,
    NgOptimizedImage,
    RelativeUrlPipe,
    UserAvatarComponent,
  ],
  imports: [
    CommonModule,
    NgOptimizedImage,
    RelativeUrlPipe,
    UserAvatarComponent,
  ],
})
export class SharedModule {}
