import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, map } from 'rxjs';
import { Comment } from '../_models/comments';

@Injectable({
  providedIn: 'root',
})
export class CommentsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getPostComments(postId: number): Observable<Comment[]> {
    return this.http
      .get<{ comments: Comment[] }>(this.baseUrl + 'comments/' + postId)
      .pipe(map((response) => response.comments));
  }

  uploadComment(postId: number, content: string): Observable<Comment> {
    return this.http.post<Comment>(this.baseUrl + 'comments/' + postId, {
      content: content,
    });
  }

  deleteComment(commentId: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'comments/' + commentId);
  }
}
