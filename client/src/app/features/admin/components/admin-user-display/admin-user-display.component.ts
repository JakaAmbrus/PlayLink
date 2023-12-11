import { Component, Input, OnInit } from '@angular/core';
import { UserWithRoles } from 'src/app/shared/models/users';
import { AdminService } from 'src/app/shared/services/admin.service';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-admin-user-display',
    templateUrl: './admin-user-display.component.html',
    styleUrl: './admin-user-display.component.scss',
    standalone: true,
    imports: [
        NgIf,
        RouterLink,
        UserAvatarComponent,
    ],
})
export class AdminUserDisplayComponent implements OnInit {
  @Input() user: UserWithRoles | undefined;

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {}

  editRole(userId: number) {
    this.adminService.editRoles(userId).subscribe({
      next: () => {
        this.user!.isModerator = !this.user!.isModerator;
      },
      error: (err) => console.error(err),
    });
  }
}
