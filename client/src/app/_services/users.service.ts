import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {
  EditUser,
  EditUserResponse,
  ProfileUser,
  SearchUser,
  User,
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
  searchUsersCache: SearchUser[] = [];
  usersCache = new Map();
  private userCache = new Map<string, ProfileUser>();

  constructor(private http: HttpClient) {}

  getUsers(userParams: UserParams): Observable<PaginatedResult<User[]>> {
    const key = Object.values(userParams).join('-');
    const cachedResponse = this.usersCache.get(key);

    if (cachedResponse) {
      return of({ ...cachedResponse });
    }

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

            const pagination = response.headers.get('Pagination');

            if (pagination) {
              this.paginatedResult.pagination = JSON.parse(
                response.headers.get('Pagination') as string
              );
            }
          }
          this.usersCache.set(key, { ...this.paginatedResult });
          return this.paginatedResult;
        })
      );
  }

  getUser(username: string): Observable<ProfileUser> {
    const cachedUser = this.userCache.get(username);
    if (cachedUser) {
      return of(cachedUser);
    }

    return this.http
      .get<{ user: ProfileUser }>(this.baseUrl + 'Users/username/' + username)
      .pipe(
        map((response) => {
          const user = response.user;
          this.userCache.set(username, user);
          return user;
        })
      );
  }

  //used when a moderator deletes a photo or description or when a user updates their own profile
  invalidateUserCache(username: string): void {
    this.userCache.delete(username);
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

  getSearchUsers(): Observable<SearchUser[]> {
    if (this.searchUsersCache && this.searchUsersCache.length > 0) {
      return of(this.searchUsersCache);
    }

    return this.http
      .get<{ users: SearchUser[] }>(this.baseUrl + 'Users/searchbar')
      .pipe(
        map((response) => response.users),
        tap((users) => (this.searchUsersCache = users))
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
