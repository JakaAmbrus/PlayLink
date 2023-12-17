import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AvatarService } from '../../shared/services/avatar.service';
import { PresenceService } from './presence.service';
import { CacheManagerService } from './cache-manager.service';
import { TokenService } from './token.service';
import { LocalStorageService } from './local-storage.service';
import {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
} from 'src/app/shared/models/auth';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  userRoles: string[] = [];

  constructor(
    private http: HttpClient,
    private presenceService: PresenceService,
    private avatarService: AvatarService,
    private cacheManagerService: CacheManagerService,
    private tokenService: TokenService,
    private localStorageService: LocalStorageService
  ) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(this.baseUrl + 'account/login', request)
      .pipe(tap((response: AuthResponse) => this.handleUserResponse(response)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(this.baseUrl + 'account/register', request)
      .pipe(tap((response: AuthResponse) => this.handleUserResponse(response)));
  }

  logout(): void {
    this.localStorageService.clearStorage();
    this.cacheManagerService.clearAllCache();
    this.avatarService.destroyAvatarDetails();
    this.setLoggedIn(false);
    this.presenceService.stopHubConnection();
  }

  private handleUserResponse(response: AuthResponse): void {
    const { username, token, fullName, gender, profilePictureUrl } =
      response.user;

    this.presenceService.createHubConnection(token);

    const roles = this.tokenService.getDecodedToken(token).role;
    this.tokenService.saveToken(token);
    this.userRoles = Array.isArray(roles) ? roles : [roles];
    this.localStorageService.setItem('roles', JSON.stringify(this.userRoles));
    this.localStorageService.setItem('username', username);
    this.setLoggedIn(true);
    this.avatarService.updateAvatarDetails({
      username,
      fullName,
      gender,
      profilePictureUrl,
    });
  }

  setLoggedIn(value: boolean): void {
    this.localStorageService.setItem('loggedIn', value ? 'true' : 'false');
  }
}
