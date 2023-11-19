import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, tap } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private loggedInStatus = new BehaviorSubject<boolean>(
    this.checkLoggedInStatus()
  );

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      tap((response: any) => {
        const user = response.user.username;
        const token = response.user.token;
        this.saveToken(token);
        this.saveUser(user);
        this.setLoggedIn(true);
      })
    );
  }
  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      tap((response: any) => {
        const user = response.username;
        const token = response.token;
        this.saveToken(token);
        this.setLoggedIn(true);
      })
    );
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

  logout() {
    localStorage.removeItem('loggedIn');
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.setLoggedIn(false);
  }

  private addAuthorizationHeader(headers: HttpHeaders) {
    const token = this.getToken();
    if (token) {
      headers = headers.set('Authorization', 'Bearer ' + token);
    }
    return headers;
  }

  getAuthenticatedData() {
    let headers = new HttpHeaders();
    headers = this.addAuthorizationHeader(headers);
    return this.http.get(this.baseUrl + 'some-endpoint', { headers });
  }
}
