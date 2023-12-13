import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ProfileUser } from '../models/users';
import { Observable, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  baseUrl = environment.apiUrl;
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
}
