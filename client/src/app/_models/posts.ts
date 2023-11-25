export interface Post {
  postId: number;
  appUserId: number;
  username: string;
  fullName: string;
  profilePictureUrl: string | null;
  gender: string;
  description: string;
  datePosted: Date;
  photoUrl: string | null;
  likesCount: number;
  commentsCount: number;
  isLikedByCurrentUser: boolean;
  isAuthorized: boolean;
}

export interface PostsResponse {
  posts: Post[];
}
