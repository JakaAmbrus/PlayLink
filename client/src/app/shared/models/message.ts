export interface Message {
  privateMessageId: number;
  senderUsername: string;
  senderFullName: string;
  senderGender: string;
  senderProfilePictureUrl: string;
  recipientUsername: string;
  recipientFullName: string;
  recipientGender: string;
  recipientProfilePictureUrl: string;
  content: string;
  dateRead: Date | null;
  privateMessageSent: Date;
}
