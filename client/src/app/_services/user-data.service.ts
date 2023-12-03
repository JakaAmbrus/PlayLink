import { Injectable, signal } from '@angular/core';
import { ProfileUser } from '../_models/users';

@Injectable({
  providedIn: 'root',
})
export class UserDataService {
  private currentUser = signal<ProfileUser>({
    appUserId: 0,
    username: '',
    gender: '',
    fullName: '',
    dateOfBirth: new Date(),
    country: '',
    profilePictureUrl: '',
    description: '',
  });

  updateCurrentUser(user: ProfileUser): void {
    this.currentUser.set(user);
  }

  updateCurrentUserDetails(
    coun: string,
    url: string | null,
    desc: string | null
  ): void {
    const currentDetails = this.currentUser();
    const updatedDetails = {
      ...currentDetails,
      country: coun,
      profilePictureUrl: url,
      description: desc,
    };
    this.currentUser.set(updatedDetails);
  }

  getCurrentUser(): ProfileUser {
    return this.currentUser();
  }
}
