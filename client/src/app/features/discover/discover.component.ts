import { Component, OnDestroy, OnInit } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { UserParams } from './models/userParams';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { UserCardComponent } from './components/user-card/user-card.component';
import { CommonModule } from '@angular/common';
import { SearchbarComponent } from './components/searchbar/searchbar.component';
import { DiscoverUsersService } from './services/discover-users.service';
import { User } from './models/discoverUser';
import { CountriesService } from './services/countries.service';
import { Subject, first, takeUntil } from 'rxjs';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';

@Component({
  selector: 'app-discover',
  templateUrl: './discover.component.html',
  styleUrls: ['./discover.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    SearchbarComponent,
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
  loadUsersError: boolean = false;
  dummyArray = new Array(6).fill(null);
  uniqueCountries: string[] = [];
  userParams: UserParams | undefined;
  private destroy$ = new Subject<void>();
  isMobileFilterVisible: boolean = false;

  constructor(
    private discoverUsersService: DiscoverUsersService,
    private countriesService: CountriesService,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit(): void {
    const savedFilters = this.localStorageService.getItem('discoverFilters');
    if (typeof savedFilters === 'string' && savedFilters) {
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

  loadUsers(): void {
    this.isLoading = true;
    this.loadUsersError = false;

    this.userParams = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      minAge: this.minAge,
      maxAge: this.maxAge,
      gender: this.gender,
      country: this.country,
      orderBy: this.orderBy,
    };

    this.discoverUsersService
      .getUsers(this.userParams)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          if (response.result && response.pagination) {
            this.users = response.result;
            this.pagination = response.pagination;
            this.isLoading = false;
          }
        },
        error: () => {
          this.isLoading = false;
          this.loadUsersError = true;
          this.users = [];
        },
      });
  }

  loadCountries(): void {
    this.countriesService
      .getUsersUniqueCountries()
      .pipe(first())
      .subscribe({
        next: (response) => {
          if (response) {
            this.uniqueCountries = response;
          }
        },
      });
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
    if (this.isMobileFilterVisible) {
      return;
    }
    if (event.key !== 'ArrowUp' && event.key !== 'ArrowDown') {
      event.preventDefault();
    }
  }

  pageChanged(event: number): void {
    this.pageNumber = event;
    this.loadUsers();
  }

  onSubmit(): void {
    this.isLoading = true;
    this.pageNumber = 1;
    if (this.isMobileFilterVisible) {
      this.isMobileFilterVisible = false;
    }
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

  showMobileFilterToggle(): void {
    this.isMobileFilterVisible = !this.isMobileFilterVisible;
  }

  ngOnDestroy() {
    this.localStorageService.setItem('discoverFilters', this.userParams);
    this.destroy$.next();
    this.destroy$.complete();
  }
}
