import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  Friend,
  FriendRequest,
  FriendRequestResponse,
  FriendshipStatusResponse,
} from '../_models/friends';

@Injectable({
  providedIn: 'root',
})
export class FriendsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUserFriends(): Observable<Friend[]> {
    return this.http.get<Friend[]>(this.baseUrl + 'friends');
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
  ): Observable<{ accepted: boolean; friend: Friend }> {
    return this.http.put<{ accepted: boolean; friend: Friend }>(
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
