import { Component, Input, OnInit } from '@angular/core';
import { UserWithRoles } from 'src/app/_models/users';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-admin-user-display',
  templateUrl: './admin-user-display.component.html',
  styleUrl: './admin-user-display.component.scss',
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
