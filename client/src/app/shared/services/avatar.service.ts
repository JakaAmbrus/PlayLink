import { Injectable, signal } from '@angular/core';
import { Avatar } from '../models/avatar';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AvatarService {
  private avatarDetails = signal<Avatar>(this.getStoredAvatarDetails());

  constructor(private localStorageService: LocalStorageService) {}

  updateAvatarDetails(details: Avatar): void {
    this.avatarDetails.set(details);
    this.localStorageService.setItem('avatar', JSON.stringify(details));
  }

  updateAvatarPhoto(url: string): void {
    const currentDetails = this.avatarDetails();
    const updatedDetails = { ...currentDetails, profilePictureUrl: url };
    this.avatarDetails.set(updatedDetails);
    this.localStorageService.setItem('avatar', JSON.stringify(updatedDetails));
  }

  private getStoredAvatarDetails(): Avatar {
    const storedDetails = this.localStorageService.getItem('avatar');
    if (typeof storedDetails === 'string') {
      return JSON.parse(storedDetails);
    }
    return { username: '', fullName: '', gender: '', profilePictureUrl: '' };
  }

  getAvatarDetails() {
    return this.avatarDetails();
  }

  destroyAvatarDetails() {
    this.avatarDetails = signal<Avatar>({
      username: '',
      fullName: '',
      gender: '',
      profilePictureUrl: '',
    });
  }
}
