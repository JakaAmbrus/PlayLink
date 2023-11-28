import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/messages';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUserMessages(pageNumber: number, pageSize: number, container: string) {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('Container', container);

    return this.http.get<{ messages: Message[] }>(
      this.baseUrl + 'messages/user',
      { observe: 'response', params }
    );
  }
}
