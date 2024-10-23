export interface Friend {
  username: string;
  fullName: string;
  profilePictureUrl: string | null;
  gender: string;
  dateEstablished: Date;
}

export interface FriendRequest {
  friendRequestId: number;
  senderUsername: string;
  senderFullName: string;
  senderProfilePictureUrl: string | null;
  senderGender: string;
  recipientUsername: string;
  recipientFullName: string;
  recipientProfilePictureUrl: string | null;
  recipientGender: string;
  status: number;
  timeSent: Date;
}

export interface FriendRequestResponse {
  friendRequestId: number;
  accept: boolean;
}

export enum FriendshipStatus {
  None,
  Friends,
  Pending,
  Declined,
}

export interface FriendshipStatusResponse {
  status: number;
}
