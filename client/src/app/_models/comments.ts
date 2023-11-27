export interface Comment {
  commentId: number;
  postId: number;
  appUserId: number;
  username: string;
  fullName: string;
  gender: string;
  profilePictureUrl: string | null;
  content: string;
  timeCommented: Date;
  likesCount: number;
  isLikedByCurrentUser: boolean;
  isAuthorized: boolean;
}

export interface CommentContent {
  content: string;
}
