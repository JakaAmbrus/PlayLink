interface Member {
  appUserId: number;
  username: string;
  fullName: string;
  age: number;
  country: string;
  city: string;
  profilePictureUrl: string | null;
}

interface MembersResponse {
  users: Member[];
}
