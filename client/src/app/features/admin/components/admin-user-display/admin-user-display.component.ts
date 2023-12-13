import { Component, Input } from '@angular/core';
import { UserWithRoles } from '../../models/user-with-roles';
import { AdminService } from 'src/app/features/admin/services/admin.service';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf } from '@angular/common';
import { first } from 'rxjs';

@Component({
  selector: 'app-admin-user-display',
  templateUrl: './admin-user-display.component.html',
  styleUrl: './admin-user-display.component.scss',
  standalone: true,
  imports: [NgIf, RouterLink, UserAvatarComponent],
})
export class AdminUserDisplayComponent {
  @Input() user: UserWithRoles | undefined;

  constructor(private adminService: AdminService) {}

  editRole(userId: number) {
    this.adminService
      .editRoles(userId)
      .pipe(first())
      .subscribe({
        next: () => {
          this.user!.isModerator = !this.user!.isModerator;
        },
        error: (err) => console.error(err),
      });
  }
}
