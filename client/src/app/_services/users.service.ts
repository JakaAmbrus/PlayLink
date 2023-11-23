import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {
  EditUser,
  EditUserResponse,
  ProfileUser,
  User,
  UsersResponse,
} from '../_models/users';
import { Observable, map, of, tap } from 'rxjs';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  baseUrl = environment.apiUrl;
  users: User[] = [];
  paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
  countriesCache: string[] = [];
  userCache = new Map();

  constructor(private http: HttpClient) {}

  getUsers(userParams: UserParams): Observable<PaginatedResult<User[]>> {
    const key = Object.values(userParams).join('-');
    const response = this.userCache.get(key);

    const params = new HttpParams()
      .set('pageNumber', userParams.pageNumber?.toString() || '')
      .set('pageSize', userParams.pageSize?.toString() || '')
      .set('Gender', userParams.gender || '')
      .set('MinAge', userParams.minAge?.toString() || '')
      .set('MaxAge', userParams.maxAge?.toString() || '')
      .set('Country', userParams.country || '')
      .set('OrderBy', userParams.orderBy || '');

    return this.http
      .get<{ users: User[]; pagination: Pagination }>(this.baseUrl + 'Users', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          if (response.body) {
            this.paginatedResult.result = response.body.users;
          }
          this.paginatedResult.result = response.body?.users;

          const pagination = response.headers.get('Pagination');
          if (pagination) {
            this.paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') as string
            );
          }
          return this.paginatedResult;
        })
      );
  }

  getUser(username: string): Observable<ProfileUser> {
    return this.http
      .get<{ user: ProfileUser }>(this.baseUrl + 'Users/username/' + username)
      .pipe(map((response) => response.user));
  }

  getUsersUniqueCountries(): Observable<string[]> {
    if (this.countriesCache && this.countriesCache.length > 0) {
      return of(this.countriesCache);
    }

    return this.http
      .get<{ countries: string[] }>(this.baseUrl + 'Users/countries')
      .pipe(
        map((response) => response.countries),
        tap((countries) => (this.countriesCache = countries))
      );
  }

  editUser(editUserData: EditUser): Observable<EditUserResponse> {
    const formData = new FormData();

    formData.append('EditUserDto.Username', editUserData.username);

    if (editUserData.image) {
      formData.append('EditUserDto.PhotoFile', editUserData.image);
    }

    if (editUserData.description) {
      formData.append('EditUserDto.Description', editUserData.description);
    }

    if (editUserData.country) {
      formData.append('EditUserDto.Country', editUserData.country);
    }

    return this.http.put<EditUserResponse>(
      this.baseUrl + 'Users/edit',
      formData
    );
  }
}
