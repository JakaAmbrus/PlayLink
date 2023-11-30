import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/messages';
import { MessageParams } from '../_models/messageParams';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  baseUrl = environment.apiUrl;
  paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<
    Message[]
  >();

  constructor(private http: HttpClient) {}

  getUserMessages(
    messageParams: MessageParams
  ): Observable<PaginatedResult<Message[]>> {
    let params = new HttpParams()
      .set('pageNumber', messageParams.pageNumber.toString())
      .set('pageSize', messageParams.pageSize.toString())
      .set('Container', messageParams.container);

    return this.http
      .get<{ messages: Message[]; pagination: Pagination }>(
        this.baseUrl + 'messages/user',
        { observe: 'response', params }
      )
      .pipe(
        map((response) => {
          if (response.body) {
            this.paginatedResult.result = response.body.messages;

            const pagination = response.headers.get('Pagination');

            if (pagination) {
              this.paginatedResult.pagination = JSON.parse(
                response.headers.get('Pagination') as string
              );
            }
          }
          return this.paginatedResult;
        })
      );
  }

  getMessageThread(username: string): Observable<Message[]> {
    return this.http
      .get<{ messages: Message[] }>(
        this.baseUrl + 'messages/thread/' + username
      )
      .pipe(map((response) => response.messages));
  }

  sendMessage(username: string, content: string) {
    return this.http
      .post<{ message: Message }>(this.baseUrl + 'messages', {
        recipientUsername: username,
        content,
      })
      .pipe(map((response) => response.message));
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
