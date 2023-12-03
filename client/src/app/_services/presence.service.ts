import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private onlineUsersSource = new BehaviorSubject<number[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();

  constructor(private toastr: ToastrService) {}

  createHubConnection(token: any) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('UserIsOnline', (id) => {
      this.onlineUsers$.pipe(take(1)).subscribe({
        next: (userIds) => {
          this.onlineUsersSource.next([...userIds, id]);
        },
      });
    });

    this.hubConnection.on('UserIsOffline', (id) => {
      this.onlineUsers$.pipe(take(1)).subscribe({
        next: (userIds) => {
          this.onlineUsersSource.next(userIds.filter((x) => x !== id));
        },
      });
    });

    this.hubConnection.on('GetOnlineUsers', (ids: number[]) => {
      this.onlineUsersSource.next(ids);
    });
  }

  stopHubConnection() {
    this.hubConnection?.stop().catch((error) => console.log(error));
  }
}
