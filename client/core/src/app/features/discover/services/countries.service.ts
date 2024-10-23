import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of, tap } from 'rxjs';
import { CacheManagerService } from 'src/app/core/services/cache-manager.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CountriesService {
  baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private cacheManager: CacheManagerService
  ) {}

  getUsersUniqueCountries(): Observable<string[]> {
    const cachedUsers = this.cacheManager.getCache<string[]>('countries');

    if (cachedUsers) {
      return of(cachedUsers);
    }

    return this.http
      .get<{ countries: string[] }>(this.baseUrl + 'Users/countries')
      .pipe(
        map((response) => response.countries),
        tap((countries) => this.cacheManager.setCache('countries', countries))
      );
  }
}
