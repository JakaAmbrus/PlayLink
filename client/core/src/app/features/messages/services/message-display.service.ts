import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Message } from 'src/app/shared/models/message';
import { PaginatedResult, Pagination } from 'src/app/shared/models/pagination';
import { environment } from 'src/environments/environment';
import { MessageParams } from '../models/messageParams';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageDisplayService {
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

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
