import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PostContent, PostsResponse } from '../_models/posts';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getPosts(): Observable<PostsResponse> {
    return this.http.get<PostsResponse>(this.baseUrl + 'posts').pipe(
      map((response) => {
        return response;
      })
    );
  }

  uploadPost(postContent: PostContent): Observable<any> {
    const formData = new FormData();
    if (postContent.description) {
      formData.append('PostContentDto.Description', postContent.description);
    }
    if (postContent.photoFile) {
      formData.append('PostContentDto.PhotoFile', postContent.photoFile);
    }

    return this.http.post(this.baseUrl + 'posts', formData);
  }

  deletePost(postId: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'posts/' + postId);
  }
}
