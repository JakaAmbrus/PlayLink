import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Post } from 'src/app/_models/posts';
import { CommentsService } from 'src/app/_services/comments.service';
import { LikesService } from 'src/app/_services/likes.service';
import { PostsService } from 'src/app/_services/posts.service';
import { Comment } from 'src/app/_models/comments';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
})
export class PostComponent {
  @Input() post: Post | undefined;

  @Output() postDeleted: EventEmitter<number> = new EventEmitter();

  comments: Comment[] = [];
  commentsShown: boolean = false;

  constructor(
    private likesService: LikesService,
    private postsService: PostsService,
    private commentsService: CommentsService
  ) {}

  toggleLike(post: Post) {
    if (post.isLikedByCurrentUser) {
      console.log('unlike');
      this.likesService.unlikePost(post.postId).subscribe(() => {
        post.isLikedByCurrentUser = false;
        post.likesCount -= 1;
      });
    } else {
      console.log('like');
      this.likesService.likePost(post.postId).subscribe(() => {
        post.isLikedByCurrentUser = true;
        post.likesCount += 1;
      });
    }
  }

  deletePost(postId: number) {
    if (confirm('Are you sure you want to delete this post?')) {
      this.postsService.deletePost(postId).subscribe(() => {
        this.post = undefined;
        this.postDeleted.emit(postId);
      });
    }
  }

  showComments(): void {
    this.commentsShown = true;
    if (this.post?.commentsCount === 0) {
      return;
    }
    this.loadComments();
  }

  loadComments() {
    if (this.post?.postId) {
      this.commentsService
        .getPostComments(this.post.postId)
        .subscribe((comments) => {
          this.comments = comments;
          console.log(this.comments);
        });
    }
  }

  onCommentUpload(comment: any) {
    this.comments = [comment.commentDto, ...this.comments];
    this.post!.commentsCount += 1;
    console.log(this.comments);
  }

  onCommentDelete(commentId: number) {
    this.comments = this.comments.filter(
      (comment) => comment.commentId !== commentId
    );
    this.post!.commentsCount -= 1;
  }
}
