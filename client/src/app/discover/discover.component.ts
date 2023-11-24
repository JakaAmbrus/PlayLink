import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { UsersService } from '../_services/users.service';
import { SearchUser, User } from '../_models/users';
import { Observable } from 'rxjs';
import { Pagination } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Component({
  selector: 'app-discover',
  templateUrl: './discover.component.html',
  styleUrls: ['./discover.component.scss'],
})
export class DiscoverComponent implements OnInit, OnDestroy {
  users: User[] = [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 6;
  minAge = 12;
  maxAge = 99;
  gender = '';
  country = '';
  orderBy = 'lastActive';
  isLoading: boolean = true;
  dummyArray = new Array(6).fill(null);
  uniqueCountries: string[] = [];
  searchUsers: SearchUser[] = [];
  userParams: UserParams | undefined;

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    const savedFilters = localStorage.getItem('discoverFilters');
    if (savedFilters) {
      const savedParams: UserParams = JSON.parse(savedFilters);
      this.userParams = JSON.parse(savedFilters);
      this.pageNumber = savedParams.pageNumber;
      this.pageSize = savedParams.pageSize;
      this.minAge = savedParams.minAge;
      this.maxAge = savedParams.maxAge;
      this.gender = savedParams.gender;
      this.country = savedParams.country;
      this.orderBy = savedParams.orderBy;
    } else {
      this.resetFilters();
    }
    this.loadUsers();
    this.loadCountries();
    this.loadSearchUsers();
  }

  loadCountries() {
    this.usersService.getUsersUniqueCountries().subscribe({
      next: (response) => {
        if (response) {
          this.uniqueCountries = response;
        }
      },
    });
  }

  loadSearchUsers() {
    this.usersService.getSearchUsers().subscribe({
      next: (response) => {
        if (response) {
          this.searchUsers = response;
        }
      },
    });
  }

  loadUsers() {
    this.userParams = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      minAge: this.minAge,
      maxAge: this.maxAge,
      gender: this.gender,
      country: this.country,
      orderBy: this.orderBy,
    };

    this.usersService.getUsers(this.userParams).subscribe({
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
    this.country = '';
    this.orderBy = 'lastActive';
  }

  ngOnDestroy() {
    localStorage.setItem('discoverFilters', JSON.stringify(this.userParams));
  }
}
