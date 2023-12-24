import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post, PostContent } from '../models/posts';
import { Observable, map } from 'rxjs';
import { PaginatedResult, Pagination } from '../models/pagination';

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  baseUrl = environment.apiUrl;
  paginatedResult: PaginatedResult<Post[]> = new PaginatedResult<Post[]>();

  constructor(private http: HttpClient) {}

  getPosts(
    pageNumber: number,
    pageSize: number
  ): Observable<PaginatedResult<Post[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http
      .get<{ posts: Post[]; pagination: Pagination }>(this.baseUrl + 'posts', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          this.paginatedResult.result = response.body?.posts;
          if (response.headers.get('Pagination')) {
            this.paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') as string
            );
          }
          return this.paginatedResult;
        })
      );
  }

  getPostsByUsername(
    username: string,
    pageNumber: number,
    pageSize: number
  ): Observable<PaginatedResult<Post[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http
      .get<{ posts: Post[]; pagination: Pagination }>(
        this.baseUrl + 'posts/user/' + username,
        {
          observe: 'response',
          params,
        }
      )
      .pipe(
        map((response) => {
          this.paginatedResult.result = response.body?.posts;
          if (response.headers.get('Pagination')) {
            this.paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') as string
            );
          }
          return this.paginatedResult;
        })
      );
  }

  uploadPost(postContent: PostContent): Observable<any> {
    const formData = new FormData();
    if (postContent.description) {
      formData.append('Description', postContent.description);
    }
    if (postContent.photoFile) {
      formData.append('PhotoFile', postContent.photoFile);
    }

    return this.http.post(this.baseUrl + 'posts', formData);
  }

  deletePost(postId: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'posts/' + postId);
  }
}
