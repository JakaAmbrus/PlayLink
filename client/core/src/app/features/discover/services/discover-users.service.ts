import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of } from 'rxjs';
import { PaginatedResult, Pagination } from 'src/app/shared/models/pagination';
import { UserParams } from 'src/app/features/discover/models/userParams';

import { environment } from 'src/environments/environment';
import { User } from '../models/discoverUser';
import { CacheManagerService } from 'src/app/core/services/cache-manager.service';

@Injectable({
  providedIn: 'root',
})
export class DiscoverUsersService {
  baseUrl = environment.apiUrl;
  paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
  usersCache = new Map();

  constructor(
    private http: HttpClient,
    private cacheManager: CacheManagerService
  ) {}

  getUsers(userParams: UserParams): Observable<PaginatedResult<User[]>> {
    const cacheKey = 'users-' + Object.values(userParams).join('-');
    const cachedResponse =
      this.cacheManager.getCache<PaginatedResult<User[]>>(cacheKey);

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
          this.cacheManager.setCache(cacheKey, { ...this.paginatedResult });
          return this.paginatedResult;
        })
      );
  }
}
