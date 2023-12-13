import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SearchUser } from '../models/users';
import { HttpClient } from '@angular/common/http';
import { Observable, map, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserSearchService {
  baseUrl = environment.apiUrl;
  searchUsersCache: SearchUser[] = [];

  constructor(private http: HttpClient) {}

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
