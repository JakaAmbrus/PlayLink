import { Component, Input } from '@angular/core';
import { UserWithRoles } from '../../models/user-with-roles';
import { AdminService } from 'src/app/features/admin/services/admin.service';
import { UserAvatarComponent } from '../../../../shared/components/user-avatar/user-avatar.component';
import { Router, RouterLink } from '@angular/router';
import { first } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from 'src/app/shared/components/dialog/dialog.component';
import { ToastrService } from 'ngx-toastr';
import { RelativeTimePipe } from '../../../../shared/pipes/relative-time.pipe';

@Component({
  selector: 'app-admin-user-display',
  templateUrl: './admin-user-display.component.html',
  styleUrl: './admin-user-display.component.scss',
  standalone: true,
  imports: [RouterLink, UserAvatarComponent, RelativeTimePipe],
})
export class AdminUserDisplayComponent {
  @Input() user: UserWithRoles | undefined;

  constructor(
    private adminService: AdminService,
    public dialog: MatDialog,
    private toastr: ToastrService,
    private router: Router
  ) {}

  editRole(userId: number) {
    const dialogRef = this.dialog.open(DialogComponent, {
      data: {
        title: 'Edit Role',
        message: 'Are you sure you want to edit the role?',
      },
    });

    dialogRef
      .afterClosed()
      .pipe(first())
      .subscribe((result) => {
        if (result) {
          this.adminService
            .editRoles(userId)
            .pipe(first())
            .subscribe({
              next: () => {
                this.router
                  .navigateByUrl('/RefreshComponent', {
                    skipLocationChange: true,
                  })
                  .then(() => {
                    this.router.navigate(['/admin']);
                  });
                this.toastr.success('Role edited successfully');
              },
            });
        }
      });
  }

  deleteUser(userId: number): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      data: {
        title: 'Delete Member',
        message: 'Are you sure you want to delete this user?',
      },
    });

    dialogRef
      .afterClosed()
      .pipe(first())
      .subscribe((result) => {
        if (result) {
          this.adminService
            .deleteUser(userId)
            .pipe(first())
            .subscribe({
              next: () => {
                this.router
                  .navigateByUrl('/RefreshComponent', {
                    skipLocationChange: true,
                  })
                  .then(() => {
                    this.router.navigate(['/admin']);
                  });
                this.toastr.success('User deleted successfully');
              },
            });
        }
      });
  }
}
