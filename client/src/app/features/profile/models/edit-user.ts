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
