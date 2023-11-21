import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {
  EditUser,
  EditUserResponse,
  ProfileUser,
  User,
  UsersResponse,
} from '../_models/users';
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

  getUser(username: string): Observable<ProfileUser> {
    return this.http
      .get<{ user: ProfileUser }>(this.baseUrl + 'Users/username/' + username)
      .pipe(map((response) => response.user));
  }

  editUser(editUserData: EditUser): Observable<EditUserResponse> {
    const formData = new FormData();

    if (editUserData.image) {
      formData.append('image', editUserData.image, editUserData.image.name);
    }

    if (editUserData.description) {
      formData.append('description', editUserData.description);
    }

    if (editUserData.country) {
      formData.append('country', editUserData.country);
    }

    return this.http.post<EditUserResponse>(
      this.baseUrl + 'Users/edit',
      formData
    );
  }
}
