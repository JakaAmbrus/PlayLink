import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {catchError, from, Observable, switchMap, tap} from 'rxjs';
import {environment} from 'src/environments/environment';
import {AvatarService} from '../../shared/services/avatar.service';
import {PresenceService} from './presence.service';
import {CacheManagerService} from './cache-manager.service';
import {TokenService} from './token.service';
import {LocalStorageService} from './local-storage.service';
import {AuthResponse, LoginRequest, RegisterRequest,} from 'src/app/shared/models/auth';
import {Auth, signInWithEmailAndPassword} from "@angular/fire/auth";
import {ToastrService} from "ngx-toastr";

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
    private localStorageService: LocalStorageService,
    private firebaseAuth: Auth,
    private toastr: ToastrService
  ) {
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    const email = `${request.username}@playlink.com`;

    return from(signInWithEmailAndPassword(this.firebaseAuth, email, request.password)).pipe(
      catchError((error) => {
        if (error.code === 'auth/invalid-credential') {
          this.toastr.error("Invalid credentials");
        } else {
          this.toastr.error("Please try again");
        }
        throw error;
      }),
      switchMap((userCredential) => {
        if (!userCredential.user) {
          this.toastr.error("Login failed: Invalid credentials");
          throw new Error("Invalid credentials");
        }

        return userCredential.user.getIdToken();
      }),
      switchMap((idToken) => {
        const headers = new HttpHeaders().set('Authorization', `Bearer ${idToken}`);
        return this.http.get<AuthResponse>(`${this.baseUrl}users/current`, {headers}).pipe(
          tap((response) => {
            this.handleUserResponse(response, idToken);
          })
        );
      })
    );
  }

  guestLogin(role: string): Observable<AuthResponse> {
    return this.http.post<{ token: string }>(`${this.baseUrl}shield/guest`, {role}).pipe(
      switchMap((response) => {
        const guestToken = response.token;
        console.log(guestToken);

        const headers = new HttpHeaders().set('Authorization', `Bearer ${guestToken}`);
        return this.http.get<AuthResponse>(`${this.baseUrl}users/current`, {headers})
          .pipe(
            tap((userResponse) => this.handleUserResponse(userResponse, guestToken))
          );
      })
    );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(this.baseUrl + 'shield/signup', request);
  }

  logout(): void {
    this.localStorageService.clearStorage();
    this.cacheManagerService.clearAllCache();
    this.avatarService.destroyAvatarDetails();
    this.setLoggedIn(false);
    this.presenceService.stopHubConnection();
  }

  deleteAccount(): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'users/delete');
  }

  setLoggedIn(value: boolean): void {
    this.localStorageService.setItem('loggedIn', value ? 'true' : 'false');
  }

  private handleUserResponse(response: AuthResponse, idToken: string): void {
    const {appUserId, username, gender, fullName, age, country, profilePictureUrl} =
      response.user;

    this.presenceService.createHubConnection(idToken);

    const roles = this.tokenService.getDecodedToken(idToken).role;
    this.tokenService.saveToken(idToken);
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
}
