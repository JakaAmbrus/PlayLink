export interface UserWithRoles {
  appUserId: number;
  username: string;
  gender: string;
  fullName: string;
  isModerator: boolean;
  profilePictureUrl: string | null;
}
