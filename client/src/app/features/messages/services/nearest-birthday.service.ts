import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { NearestBirthdayUser } from '../models/nearestBirthdayUser';
import { Observable, map, of, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CacheManagerService } from 'src/app/core/services/cache-manager.service';

@Injectable({
  providedIn: 'root',
})
export class NearestBirthdayService {
  baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private cacheManager: CacheManagerService
  ) {}

  getNearestBirthdayUsers(): Observable<NearestBirthdayUser[]> {
    const cachedUsers = this.cacheManager.getCache<NearestBirthdayUser[]>(
      'nearestBirthdayUsers'
    );
    if (cachedUsers) {
      return of(cachedUsers);
    }

    return this.http
      .get<{ users: NearestBirthdayUser[] }>(
        this.baseUrl + 'Users/nearest-birthday'
      )
      .pipe(
        map((response) => response.users),
        tap((users) =>
          this.cacheManager.setCache('nearestBirthdayUsers', users)
        )
      );
  }
}
