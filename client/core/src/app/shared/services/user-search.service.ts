import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SearchUser } from '../models/users';
import { HttpClient } from '@angular/common/http';
import { Observable, map, of, tap } from 'rxjs';
import { CacheManagerService } from 'src/app/core/services/cache-manager.service';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class UserSearchService {
  baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private cacheManager: CacheManagerService,
    private localStorageService: LocalStorageService
  ) {}

  getSearchUsers(): Observable<SearchUser[]> {
    const cachedUsers = this.cacheManager.getCache<SearchUser[]>('searchUsers');

    if (cachedUsers) {
      return of(cachedUsers);
    }

    const username = this.localStorageService.getItem<string>('username');

    return this.http
      .get<{ users: SearchUser[] }>(this.baseUrl + 'Users/searchbar')
      .pipe(
        map((response) =>
          response.users.filter((user) => user.username !== username)
        ),
        tap((users) => this.cacheManager.setCache('searchUsers', users))
      );
  }
}
