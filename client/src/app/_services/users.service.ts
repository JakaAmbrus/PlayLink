import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User, UsersResponse } from '../_models/users';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http
      .get<UsersResponse>(this.baseUrl + 'Users')
      .pipe(map((response) => response.users));
  }

  getUser(id: number) {
    return this.http.get<User>(this.baseUrl + 'Users/' + id);
  }
}
