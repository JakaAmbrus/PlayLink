import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { NearestBirthdayUser } from '../models/nearestBirthdayUser';
import { Observable, map, of, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class NearestBirthdayService {
  baseUrl = environment.apiUrl;
  nearestBirthdayUsersCache: NearestBirthdayUser[] = [];

  constructor(private http: HttpClient) {}

  getNearestBirthdayUsers(): Observable<NearestBirthdayUser[]> {
    if (
      this.nearestBirthdayUsersCache &&
      this.nearestBirthdayUsersCache.length > 0
    ) {
      return of(this.nearestBirthdayUsersCache);
    }

    return this.http
      .get<{ users: NearestBirthdayUser[] }>(
        this.baseUrl + 'Users/nearest-birthday'
      )
      .pipe(
        map((response) => response.users),
        tap((users) => (this.nearestBirthdayUsersCache = users))
      );
  }
}
