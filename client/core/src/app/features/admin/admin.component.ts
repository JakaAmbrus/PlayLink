import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserWithRoles } from './models/user-with-roles';
import { AdminService } from './services/admin.service';
import { Pagination } from '../../shared/models/pagination';
import { NgxPaginationModule } from 'ngx-pagination';
import { AdminUserDisplayComponent } from './components/admin-user-display/admin-user-display.component';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
  standalone: true,
  imports: [AdminUserDisplayComponent, NgxPaginationModule],
})
export class AdminComponent implements OnInit, OnDestroy {
  users: UserWithRoles[] = [];
  pageNumber: number = 1;
  pageSize: number = 6;
  totalUsers: number | undefined;
  pagination: Pagination | undefined;
  private destroy$ = new Subject<void>();

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.adminService
      .getUsersWithRoles(this.pageNumber, this.pageSize)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          const loadedUsers = response.result;
          if (loadedUsers && response.pagination) {
            this.users = loadedUsers;
            this.totalUsers = response.pagination?.totalItems;
            this.pagination = response.pagination;
          }
        },
        error: (err) => console.error(err),
      });
  }

  pageChanged(event: number): void {
    this.pageNumber = event;
    this.loadUsers();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
