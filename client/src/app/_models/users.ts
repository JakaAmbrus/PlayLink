export interface User {
  appUserId: number;
  username: string;
  gender: string;
  fullName: string;
  age: number;
  country: string;
  profilePictureUrl: string | null;
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
export interface UsersResponse {
  users: User[];
}
