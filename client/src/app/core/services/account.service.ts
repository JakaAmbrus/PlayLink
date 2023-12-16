import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AvatarService } from '../../shared/services/avatar.service';
import { PresenceService } from '../../shared/services/presence.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  userRoles: string[] = [];
  private loggedInStatus = new BehaviorSubject<boolean>(
    this.checkLoggedInStatus()
  );

  constructor(
    private http: HttpClient,
    private presenceService: PresenceService,
    private avatarService: AvatarService
  ) {}

  login(model: any) {
    return this.http
      .post(this.baseUrl + 'account/login', model)
      .pipe(tap((response) => this.handleUserResponse(response)));
  }

  register(model: any) {
    return this.http
      .post(this.baseUrl + 'account/register', model)
      .pipe(tap((response) => this.handleUserResponse(response)));
  }

  private handleUserResponse(response: any) {
    const { username, token, fullName, gender, profilePictureUrl } =
      response.user;

    this.presenceService.createHubConnection(token);

    const roles = this.getDecodedToken(token).role;
    this.userRoles = Array.isArray(roles) ? roles : [roles];
    localStorage.setItem('roles', JSON.stringify(this.userRoles));
    this.saveToken(token);
    this.saveUser(username);
    this.setLoggedIn(true);
    this.avatarService.updateAvatarDetails({
      username,
      fullName,
      gender,
      profilePictureUrl,
    });
  }

  logout() {
    localStorage.clear();
    this.setLoggedIn(false);
    this.presenceService.stopHubConnection();
  }

  saveUser(user: string) {
    localStorage.setItem('user', user);
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }

  get isLoggedIn() {
    return this.loggedInStatus.asObservable();
  }

  setLoggedIn(value: boolean) {
    this.loggedInStatus.next(value);
    localStorage.setItem('loggedIn', value ? 'true' : 'false');
  }

  private checkLoggedInStatus(): boolean {
    const loggedIn = localStorage.getItem('loggedIn');
    return loggedIn === 'true';
  }
}