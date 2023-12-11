import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Post } from 'src/app/shared/models/posts';
import { CommentsService } from 'src/app/shared/services/comments.service';
import { LikesService } from 'src/app/shared/services/likes.service';
import { PostsService } from 'src/app/shared/services/posts.service';
import { Comment } from 'src/app/shared/models/comments';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { RelativeTimePipe } from '../../pipes/relative-time.pipe';
import { RelativeUrlPipe } from '../../pipes/relative-url.pipe';
import { UploadCommentComponent } from '../upload-comment/upload-comment.component';
import { CommentComponent } from '../comment/comment.component';
import { UserAvatarComponent } from '../user-avatar/user-avatar.component';
import { RouterLink } from '@angular/router';
import { NgIf, NgClass, NgOptimizedImage, NgFor } from '@angular/common';

@Component({
    selector: 'app-post',
    templateUrl: './post.component.html',
    styleUrls: ['./post.component.scss'],
    standalone: true,
    imports: [
        NgIf,
        RouterLink,
        UserAvatarComponent,
        NgClass,
        NgOptimizedImage,
        NgFor,
        CommentComponent,
        UploadCommentComponent,
        RelativeUrlPipe,
        RelativeTimePipe,
    ],
})
export class PostComponent {
  @Input() post: Post | undefined;

  @Output() postDeleted: EventEmitter<number> = new EventEmitter();

  comments: Comment[] = [];
  commentsShown: boolean = false;

  constructor(
    private likesService: LikesService,
    private postsService: PostsService,
    private commentsService: CommentsService,
    public dialog: MatDialog
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
    const dialogRef = this.dialog.open(DialogComponent, {
      data: {
        title: 'Delete Post',
        message: 'Are you sure you want to delete this post?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.postsService.deletePost(postId).subscribe(() => {
          this.post = undefined;
          this.postDeleted.emit(postId);
        });
      }
    });
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
