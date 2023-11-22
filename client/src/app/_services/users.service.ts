import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {
  EditUser,
  EditUserResponse,
  ProfileUser,
  User,
  UsersResponse,
} from '../_models/users';
import { Observable, map, of } from 'rxjs';
import { PaginatedResult, Pagination } from '../_models/pagination';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  baseUrl = environment.apiUrl;
  users: User[] = [];
  paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

  constructor(private http: HttpClient) {}

  getUsers(
    page?: number,
    itemsPerPage?: number
  ): Observable<PaginatedResult<User[]>> {
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<{ users: User[]; pagination: Pagination }>(this.baseUrl + 'Users', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          if (response.body) {
            this.paginatedResult.result = response.body.users;
          }
          this.paginatedResult.result = response.body?.users;

          const pagination = response.headers.get('Pagination');
          if (pagination) {
            this.paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') as string
            );
          }
          return this.paginatedResult;
        })
      );
  }

  getUser(username: string): Observable<ProfileUser> {
    return this.http
      .get<{ user: ProfileUser }>(this.baseUrl + 'Users/username/' + username)
      .pipe(map((response) => response.user));
  }

  editUser(editUserData: EditUser): Observable<EditUserResponse> {
    const formData = new FormData();

    formData.append('EditUserDto.Username', editUserData.username);

    if (editUserData.image) {
      formData.append('EditUserDto.PhotoFile', editUserData.image);
    }

    if (editUserData.description) {
      formData.append('EditUserDto.Description', editUserData.description);
    }

    if (editUserData.country) {
      formData.append('EditUserDto.Country', editUserData.country);
    }

    return this.http.put<EditUserResponse>(
      this.baseUrl + 'Users/edit',
      formData
    );
  }
}
