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
  dateOfBirth: string;
  country: string;
  profilePictureUrl: string | null;
  description: string | null;
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
  country: string | null;
}

export interface SearchUser {
  username: string;
  fullName: string;
  profilePictureUrl: string | null;
}
