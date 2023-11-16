export interface User {
  appUserId: number;
  username: string;
  fullName: string;
  age: number;
  country: string;
  city: string;
  profilePictureUrl: string | null;
}

export interface UsersResponse {
  users: User[];
}
