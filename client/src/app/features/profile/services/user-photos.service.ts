import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserPhotosService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUserPhotos(username: string): Observable<string[]> {
    return this.http
      .get<{ photos: string[] }>(
        this.baseUrl + 'posts/user/' + username + '/photos'
      )
      .pipe(map((response) => response.photos));
  }
}
