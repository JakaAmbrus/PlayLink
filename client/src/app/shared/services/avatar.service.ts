import { Injectable, signal } from '@angular/core';
import { Avatar } from '../models/avatar';

@Injectable({
  providedIn: 'root',
})
export class AvatarService {
  private avatarDetails = signal<Avatar>(this.getStoredAvatarDetails());

  updateAvatarDetails(details: Avatar): void {
    this.avatarDetails.set(details);
    localStorage.setItem('avatar', JSON.stringify(details));
  }

  updateAvatarPhoto(url: string): void {
    const currentDetails = this.avatarDetails();
    const updatedDetails = { ...currentDetails, pictureUrl: url };
    this.avatarDetails.set(updatedDetails);
    localStorage.setItem('avatar', JSON.stringify(updatedDetails));
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
