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
  username: string;
  fullName: string;
  profilePictureUrl: string | null;
}

export interface UserWithRoles {
  appUserId: number;
  username: string;
  gender: string;
  fullName: string;
  isModerator: boolean;
  profilePictureUrl: string | null;
}
