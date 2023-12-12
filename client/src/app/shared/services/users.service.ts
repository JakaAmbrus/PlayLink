import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ProfileUser, SearchUser } from '../models/users';
import { Observable, map, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  baseUrl = environment.apiUrl;
  countriesCache: string[] = [];
  searchUsersCache: SearchUser[] = [];
  private userCache = new Map<string, ProfileUser>();

  constructor(private http: HttpClient) {}

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
}
