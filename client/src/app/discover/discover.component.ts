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
  users: User[] = [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 6;
  minAge = 12;
  maxAge = 99;
  gender = '';
  country = '';
  isLoading: boolean = true;
  dummyArray = new Array(6).fill(null);
  uniqueCountries: string[] = [];

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.loadUsers();
    this.loadCountries();
  }

  loadCountries() {
    this.usersService.getUsersUniqueCountries().subscribe({
      next: (response) => {
        this.uniqueCountries = response;
      },
    });
  }

  loadUsers() {
    const userParams = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      minAge: this.minAge,
      maxAge: this.maxAge,
      gender: this.gender,
      country: this.country,
    };

    this.usersService.getUsers(userParams).subscribe({
      next: (response) => {
        if (response.result && response.pagination) {
          this.users = response.result;
          this.pagination = response.pagination;
          this.isLoading = false;
        }
      },
    });
  }

  pageChanged(event: any): void {
    this.pageNumber = event;
    this.loadUsers();
  }

  onSubmit(): void {
    this.isLoading = true;
    this.pageNumber = 1;
    this.loadUsers();
  }
  resetFilters(): void {
    this.pageNumber = 1;
    this.minAge = 12;
    this.maxAge = 99;
    this.gender = '';
  }
}
