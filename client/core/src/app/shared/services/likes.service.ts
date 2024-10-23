import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LikedUser } from '../models/likedUser';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getPostLikes(postId: number): Observable<LikedUser[]> {
    return this.http
      .get<{ likedUsers: LikedUser[] }>(
        this.baseUrl + 'posts/' + postId + '/likes'
      )
      .pipe(map((result) => result.likedUsers));
  }

  likePost(postId: number): Observable<any> {
    return this.http.post(this.baseUrl + 'posts/' + postId + '/like', {});
  }

  unlikePost(postId: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'posts/' + postId + '/like');
  }

  getCommentLikes(commentId: number): Observable<LikedUser[]> {
    return this.http
      .get<{ likedUsers: LikedUser[] }>(
        this.baseUrl + 'comments/' + commentId + '/likes'
      )
      .pipe(map((result) => result.likedUsers));
  }

  likeComment(commentId: number): Observable<any> {
    return this.http.post(this.baseUrl + 'comments/' + commentId + '/like', {});
  }

  unlikeComment(commentId: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'comments/' + commentId + '/like');
  }
}
