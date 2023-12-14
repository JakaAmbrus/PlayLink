import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  Friend,
  FriendRequest,
  FriendRequestResponse,
  FriendshipStatusResponse,
} from '../models/friends';
import { CacheManagerService } from 'src/app/core/services/cache-manager.service';

@Injectable({
  providedIn: 'root',
})
export class FriendsService {
  baseUrl = environment.apiUrl;
  private friendsSubject = new BehaviorSubject<Friend[]>([]);
  friends$ = this.friendsSubject.asObservable();

  constructor(
    private http: HttpClient,
    private cacheManager: CacheManagerService
  ) {}

  getFriends(forceRefresh: boolean = false): Observable<Friend[]> {
    if (!forceRefresh) {
      const cachedFriends = this.cacheManager.getCache<Friend[]>('friends');
      if (cachedFriends) {
        return of(cachedFriends);
      }
    }
    return this.http.get<{ friends: Friend[] }>(this.baseUrl + 'friends').pipe(
      tap((result) => {
        this.cacheManager.setCache('friends', result.friends);
        this.friendsSubject.next(result.friends);
      }),
      map((result) => result.friends)
    );
  }

  addFriend(newFriend: Friend): void {
    this.getFriends().subscribe((friends) => {
      const updatedFriends = [...friends, newFriend];
      this.cacheManager.setCache('friends', updatedFriends);
      this.friendsSubject.next(updatedFriends);
    });
  }

  getFriendRequests(): Observable<{ friendRequests: FriendRequest[] }> {
    return this.http.get<{ friendRequests: FriendRequest[] }>(
      this.baseUrl + 'friends/requests'
    );
  }

  getFriendRequestStatus(
    username: string
  ): Observable<FriendshipStatusResponse> {
    return this.http.get<FriendshipStatusResponse>(
      this.baseUrl + 'friends/status/' + username
    );
  }

  sendFriendRequest(username: string): Observable<void> {
    return this.http.post<void>(this.baseUrl + 'friends/' + username, {});
  }

  respondToFriendRequest(
    responseDto: FriendRequestResponse
  ): Observable<{ requestAccepted: boolean; friendDto: Friend }> {
    return this.http.put<{ requestAccepted: boolean; friendDto: Friend }>(
      this.baseUrl + 'friends/request-response',
      responseDto
    );
  }

  removeFriendship(username: string): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'friends/' + username).pipe(
      tap(() => {
        this.getFriends(true).subscribe();
      })
    );
  }

  removeFriendRequest(requestId: number): Observable<void> {
    return this.http.delete<void>(
      this.baseUrl + 'friends/request/' + requestId
    );
  }
}
