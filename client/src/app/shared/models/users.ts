export interface User {
  appUserId: number;
  username: string;
  gender: string;
  fullName: string;
  age: number;
  country: string;
  profilePictureUrl: string | null;
}

export interface UsersResponse {
  users: User[];
}

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

export interface EditUser {
  username: string;
  image: File | null;
  description: string | null;
  country: string | null;
}
export interface EditUserResponse {
  photoUrl: string | null;
  description: string | null;
  country: string;
}

export interface SearchUser {
  appUserId: number;
  username: string;
  fullName: string;
  profilePictureUrl: string | null;
  gender: string;
}

export interface NearestBirthdayUser {
  username: string;
  fullName: string;
  profilePictureUrl: string | null;
  gender: string;
  dateOfBirth: Date;
  daysUntilBirthday: number;
}
