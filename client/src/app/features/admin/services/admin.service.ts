import { Injectable } from '@angular/core';
import { PaginatedResult, Pagination } from '../../../shared/models/pagination';
import { UserWithRoles } from '../models/user-with-roles';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl;
  paginatedResult: PaginatedResult<UserWithRoles[]> = new PaginatedResult<
    UserWithRoles[]
  >();

  constructor(private http: HttpClient) {}

  getUsersWithRoles(
    pageNumber: number,
    pageSize: number
  ): Observable<PaginatedResult<UserWithRoles[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http
      .get<{ users: UserWithRoles[]; pagination: Pagination }>(
        this.baseUrl + 'admin/users',
        {
          observe: 'response',
          params,
        }
      )
      .pipe(
        map((response) => {
          this.paginatedResult.result = response.body?.users;
          if (response.headers.get('Pagination')) {
            this.paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') as string
            );
          }
          return this.paginatedResult;
        })
      );
  }

  editRoles(userId: number) {
    return this.http.put(this.baseUrl + 'admin/edit-roles/' + userId, {});
  }

  deleteUser(userId: number) {
    return this.http.delete(this.baseUrl + 'admin/delete-user/' + userId);
  }
}
