import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/messages';
import { MessageParams } from '../_models/messageParams';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { BehaviorSubject, Observable, map, take } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
  paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<
    Message[]
  >();

  constructor(private http: HttpClient) {}

  createHubConnection(token: any, otherUsername: string): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', (response) => {
      this.messageThreadSource.next(response.messages);
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some((x) => x.username === otherUsername)) {
        this.messageThread$.pipe(take(1)).subscribe((messages) => {
          messages.forEach((message) => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now());
            }
          });

          this.messageThreadSource.next([...messages]);
        });
      }
    });

    this.hubConnection.on('NewMessage', (message) => {
      this.messageThread$.pipe(take(1)).subscribe((messages) => {
        this.messageThreadSource.next([...messages, message]);
      });
    });
  }

  stopHubConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop().catch((error) => console.log(error));
    }
  }

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

  async sendMessageThroughHub(username: string, content: string) {
    console.log(username, content);

    return this.hubConnection
      ?.invoke('SendMessage', {
        recipientUsername: username,
        content,
      })
      .catch((error) => console.log(error));
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
