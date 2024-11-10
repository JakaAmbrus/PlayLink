import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {from, Observable, switchMap, tap} from 'rxjs';
import {environment} from 'src/environments/environment';
import {AvatarService} from '../../shared/services/avatar.service';
import {PresenceService} from './presence.service';
import {CacheManagerService} from './cache-manager.service';
import {TokenService} from './token.service';
import {LocalStorageService} from './local-storage.service';
import {AuthResponse, LoginRequest, RegisterRequest,} from 'src/app/shared/models/auth';
import {Auth, signInWithEmailAndPassword} from "@angular/fire/auth";

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
  ) {
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    const email = `${request.username}@playlink.com`;
    console.log(`Attempting to log in with email: ${email}`);

    return from(signInWithEmailAndPassword(this.firebaseAuth, email, request.password)).pipe(
      switchMap((userCredential) => {
        console.log("Firebase signInWithEmailAndPassword successful:", userCredential);
        return userCredential.user?.getIdToken() || Promise.reject('No token found');
      }),
      switchMap((idToken) => {
        console.log("Retrieved ID Token:", idToken);

        const headers = new HttpHeaders().set('Authorization', `Bearer ${idToken}`);
        console.log("Sending request to /users/current with headers:", headers);

        return this.http.get<AuthResponse>(`${this.baseUrl}users/current`, {headers}).pipe(
          tap((response) => {
            console.log("Received response from /users/current:", response);
            this.handleUserResponse(response, idToken);
          })
        );
      }),
      tap({
        complete: () => console.log("Login process completed successfully."),
        error: (error) => console.error("Error during login process:", error)
      })
    );
  }

  guestLogin(role: string): Observable<AuthResponse> {
    return this.http.post<{ token: string }>(`${this.baseUrl}shield/guest`, {role}).pipe(
      switchMap((response) => {
        const guestToken = response.token;

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
