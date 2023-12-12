import { Component, OnDestroy, OnInit } from '@angular/core';
import { UsersService } from '../../shared/services/users.service';
import { Pagination } from '../../shared/models/pagination';
import { UserParams } from './models/userParams';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { UserCardComponent } from './components/user-card/user-card.component';
import { NgIf, NgFor } from '@angular/common';
import { SearchbarComponent } from './components/searchbar/searchbar.component';
import { DiscoverUsersService } from './services/discover-users.service';
import { User } from './models/discoverUser';

@Component({
  selector: 'app-discover',
  templateUrl: './discover.component.html',
  styleUrls: ['./discover.component.scss'],
  standalone: true,
  imports: [
    SearchbarComponent,
    NgIf,
    NgFor,
    UserCardComponent,
    NgxPaginationModule,
    FormsModule,
    MatButtonToggleModule,
  ],
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
  userParams: UserParams | undefined;

  constructor(
    private usersService: UsersService,
    private discoverUsersService: DiscoverUsersService
  ) {}

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
  }

  ngOnDestroy() {
    localStorage.setItem('discoverFilters', JSON.stringify(this.userParams));
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

    this.discoverUsersService.getUsers(this.userParams).subscribe({
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
