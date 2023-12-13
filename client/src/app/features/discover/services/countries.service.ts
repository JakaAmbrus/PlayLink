import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CountriesService {
  baseUrl = environment.apiUrl;
  countriesCache: string[] = [];

  constructor(private http: HttpClient) {}

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
}
