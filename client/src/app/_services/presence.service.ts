import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
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

  constructor(private toastr: ToastrService, private router: Router) {}

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

    this.hubConnection.on('NewMessageReceived', ({ username, fullName }) => {
      this.toastr
        .info(fullName + ' has sent you a new message!')
        .onTap.pipe(take(1))
        .subscribe({
          next: () => {
            this.router.navigateByUrl('/user/' + username + '/message');
          },
        });
    });
  }

  stopHubConnection() {
    this.hubConnection?.stop().catch((error) => console.log(error));
  }
}
