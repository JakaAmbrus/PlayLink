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
    username: string;
    token: string;
    fullName: string;
    gender: string;
    profilePictureUrl: string | null;
  };
}
