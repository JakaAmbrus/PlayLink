import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ModeratorService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  deleteUserPhoto(username: string): Observable<void> {
    return this.http.delete<void>(
      this.baseUrl + 'moderator/delete-user-photo/' + username
    );
  }

  deleteUserDescription(username: string): Observable<void> {
    return this.http.delete<void>(
      this.baseUrl + 'moderator/delete-user-description/' + username
    );
  }
}
