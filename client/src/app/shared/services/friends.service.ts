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

@Injectable({
  providedIn: 'root',
})
export class FriendsService {
  baseUrl = environment.apiUrl;
  private friendsSubject = new BehaviorSubject<Friend[]>([]);
  friends$ = this.friendsSubject.asObservable();
  private friendsCache: Friend[] | null = null;

  constructor(private http: HttpClient) {}

  getFriends(forceRefresh: boolean = false): Observable<Friend[]> {
    if (!forceRefresh && this.friendsCache) {
      return of(this.friendsCache);
    } else {
      return this.http
        .get<{ friends: Friend[] }>(this.baseUrl + 'friends')
        .pipe(
          tap((result) => {
            this.friendsCache = result.friends;
            this.friendsSubject.next(result.friends);
          }),
          map((result) => result.friends)
        );
    }
  }

  addFriend(newFriend: Friend): void {
    console.log('New friend being added:', newFriend);
    if (this.friendsCache) {
      this.friendsCache = [...this.friendsCache, newFriend];
      this.friendsSubject.next(this.friendsCache);
    } else {
      this.getFriends(true).subscribe();
    }
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
    return this.http.delete<void>(this.baseUrl + 'friends/' + username);
  }

  removeFriendRequest(requestId: number): Observable<void> {
    return this.http.delete<void>(
      this.baseUrl + 'friends/request/' + requestId
    );
  }
}
