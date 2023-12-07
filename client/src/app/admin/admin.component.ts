import { Component, OnInit } from '@angular/core';
import { UserWithRoles } from '../_models/users';
import { AdminService } from '../_services/admin.service';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
})
export class AdminComponent implements OnInit {
  users: UserWithRoles[] = [];
  pageNumber: number = 1;
  pageSize: number = 6;
  totalUsers: number | undefined;
  pagination: Pagination | undefined;

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.adminService
      .getUsersWithRoles(this.pageNumber, this.pageSize)
      .subscribe({
        next: (response) => {
          const loadedUsers = response.result;
          if (loadedUsers) {
            this.users.push(...loadedUsers);
            this.totalUsers = response.pagination?.totalItems;
            this.pagination = response.pagination;
            console.log(this.users);
          }
        },
        error: (err) => console.error(err),
      });
  }

  pageChanged(event: any): void {
    this.pageNumber = event;
    this.loadUsers();
  }
}