import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  likePost(postId: number): Observable<any> {
    console.log('likePost');
    return this.http.post(this.baseUrl + 'posts/' + postId + '/like', {});
  }

  unlikePost(postId: number): Observable<any> {
    console.log('unlikePost');
    return this.http.delete(this.baseUrl + 'posts/' + postId + '/like');
  }

  likeComment(commentId: number): Observable<any> {
    console.log('likeComment');
    return this.http.post(this.baseUrl + 'comments/' + commentId + '/like', {});
  }

  unlikeComment(commentId: number): Observable<any> {
    console.log('unlikeComment');
    return this.http.delete(this.baseUrl + 'comments/' + commentId + '/like');
  }
}
