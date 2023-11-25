import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PostsResponse } from '../_models/posts';
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
}
