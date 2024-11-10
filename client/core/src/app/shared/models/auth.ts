export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
  gender: string;
  fullName: string;
  country: string;
  dateOfBirth: Date;
}

export interface AuthResponse {
  user: {
    appUserId: number,
    username: string;
    gender: string;
    fullName: string;
    age: number,
    country: string,
    profilePictureUrl: string | null;
  };
}
