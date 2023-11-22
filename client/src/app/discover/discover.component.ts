import { Component, Input, OnInit } from '@angular/core';
import { UsersService } from '../_services/users.service';
import { User } from '../_models/users';
import { Observable } from 'rxjs';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-discover',
  templateUrl: './discover.component.html',
  styleUrls: ['./discover.component.scss'],
})
export class DiscoverComponent implements OnInit {
  // users$: Observable<User[]> | undefined;
  users: User[] = [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 6;

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  // loadUsers() {
  //   this.users$ = this.usersService.getUsers();
  // }

  loadUsers() {
    this.usersService.getUsers(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        if (response.result && response.pagination) {
          this.users = response.result;
          this.pagination = response.pagination;
        }
      },
    });
  }

  pageChanged(event: any) {
    this.pageNumber = event;
    this.loadUsers();
  }
}
