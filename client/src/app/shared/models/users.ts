export interface ProfileUser {
  appUserId: number;
  username: string;
  gender: string;
  fullName: string;
  dateOfBirth: Date;
  country: string;
  profilePictureUrl: string | null;
  description: string | null;
  created: Date;
  lastActive: Date;
  authorized: boolean;
}

export interface SearchUser {
  appUserId: number;
  username: string;
  fullName: string;
  profilePictureUrl: string | null;
  gender: string;
}
