import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { EditUser, EditUserResponse } from '../models/edit-user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EditUserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

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
