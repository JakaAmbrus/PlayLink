import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { UsersService } from '../_services/users.service';
import { SearchUser, User } from '../_models/users';
import { Pagination } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-discover',
  templateUrl: './discover.component.html',
  styleUrls: ['./discover.component.scss'],
})
export class DiscoverComponent implements OnInit, OnDestroy {
  users: User[] = [];
  pagination: Pagination | undefined;
  pageNumber: number = 1;
  pageSize: number = 6;
  minAge: number = 12;
  maxAge: number = 99;
  gender: string = '';
  country: string = '';
  orderBy: string = 'lastActive';
  isLoading: boolean = true;
  dummyArray = new Array(6).fill(null);
  uniqueCountries: string[] = [];
  searchUsers: SearchUser[] = [];
  filteredUsers: SearchUser[] = [];
  searchControl = new FormControl();
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

    this.searchControl.valueChanges.subscribe((val) => {
      this.filterUsers(val);
    });

    this.loadSearchUsers();
  }

  filterUsers(val: string) {
    if (!val) {
      this.filteredUsers = this.searchUsers;
    } else {
      this.filteredUsers = this.searchUsers.filter((user) =>
        user.fullName.toLowerCase().includes(val.toLowerCase())
      );
    }
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

  validateAgeMaxRange() {
    if (this.minAge > this.maxAge) {
      this.maxAge = this.minAge;
    }
  }

  validateAgeMinRange() {
    if (this.minAge > this.maxAge) {
      this.minAge = this.maxAge;
    }
  }

  preventTyping(event: KeyboardEvent): void {
    if (event.key !== 'ArrowUp' && event.key !== 'ArrowDown') {
      event.preventDefault();
    }
  }
}
