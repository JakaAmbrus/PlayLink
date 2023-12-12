export interface NearestBirthdayUser {
  username: string;
  fullName: string;
  profilePictureUrl: string | null;
  gender: string;
  dateOfBirth: Date;
  daysUntilBirthday: number;
}
