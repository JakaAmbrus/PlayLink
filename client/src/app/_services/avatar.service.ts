import { Injectable, signal } from '@angular/core';
import { Avatar } from '../_models/avatar';

@Injectable({
  providedIn: 'root',
})
export class AvatarService {
  private avatarDetails = signal<Avatar>(this.getStoredAvatarDetails());

  constructor() {}

  updateAvatarDetails(details: Avatar): void {
    this.avatarDetails.set(details);
    localStorage.setItem('avatar', JSON.stringify(details));
  }

  private getStoredAvatarDetails(): Avatar {
    const storedDetails = localStorage.getItem('avatar');
    return storedDetails
      ? JSON.parse(storedDetails)
      : { pictureUrl: '', fullName: '', gender: '' };
  }

  getAvatarDetails() {
    return this.avatarDetails();
  }
}
