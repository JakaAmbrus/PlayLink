import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl: string = 'https://localhost:7074/api/';
  private loggedInStatus = new BehaviorSubject<boolean>(
    this.checkLoggedInStatus()
  );

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model);
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
    this.setLoggedIn(false);
  }
}
