import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor(private localStorageService: LocalStorageService) {}

  saveToken(token: string) {
    this.localStorageService.setItem('token', token);
  }

  getToken(): string | null {
    return this.localStorageService.getItem<string>('token');
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
