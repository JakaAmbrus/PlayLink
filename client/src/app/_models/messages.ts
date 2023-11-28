export interface Message {
  privateMessageId: number;
  senderId: number;
  recipientId: number;
  senderProfilePictureUrl: string;
  recipientProfilePictureUrl: string;
  senderUsername: string;
  recipientUsername: string;
  content: string;
  dateRead?: Date;
  privateMessageSent: Date;
}
