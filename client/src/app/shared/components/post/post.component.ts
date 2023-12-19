import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
} from '@angular/core';
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
import { Subject, first, takeUntil } from 'rxjs';
import { LikedUser } from '../../models/likedUser';
import { LikedUsersListComponent } from '../liked-users-list/liked-users-list.component';
import { ClickOutsideService } from '../../services/click-outside.service';

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
    LikedUsersListComponent,
  ],
})
export class PostComponent implements OnDestroy {
  @Input() post: Post | undefined;

  @Output() postDeleted: EventEmitter<number> = new EventEmitter();

  comments: Comment[] = [];
  commentsShown: boolean = false;
  pageNumber: number = 1;
  pageSize: number = 3;
  totalComments: number | undefined;
  allCommentsLoaded: boolean = false;
  likedUsers: LikedUser[] = [];
  showLikedUsers: boolean = false;
  private destroy$ = new Subject<void>();

  constructor(
    private likesService: LikesService,
    private postsService: PostsService,
    private commentsService: CommentsService,
    public dialog: MatDialog,
    private clickOutsideService: ClickOutsideService
  ) {}

  displayLikedUsers(): void {
    if (this.post?.likesCount === 0) {
      return;
    }
    this.showLikedUsers = !this.showLikedUsers;
    if (this.showLikedUsers) {
      this.loadLikedUsers();
      this.clickOutsideService.bind(this, () => {
        this.showLikedUsers = false;
      });
    } else {
      this.clickOutsideService.unbind(this);
    }
  }

  loadLikedUsers(): void {
    if (this.post?.postId) {
      this.likesService
        .getPostLikes(this.post.postId)
        .pipe(takeUntil(this.destroy$))
        .subscribe((likedUsers) => {
          this.likedUsers = likedUsers;
        });
    }
  }

  toggleLike(post: Post): void {
    if (post.isLikedByCurrentUser) {
      this.likesService
        .unlikePost(post.postId)
        .pipe(first())
        .subscribe(() => {
          post.isLikedByCurrentUser = false;
          post.likesCount -= 1;
        });
    } else {
      this.likesService
        .likePost(post.postId)
        .pipe(first())
        .subscribe(() => {
          post.isLikedByCurrentUser = true;
          post.likesCount += 1;
        });
    }
  }

  deletePost(postId: number): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      data: {
        title: 'Delete Post',
        message: 'Are you sure you want to delete this post?',
      },
    });

    dialogRef
      .afterClosed()
      .pipe(first())
      .subscribe((result) => {
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

  loadComments(): void {
    if (this.post?.postId) {
      this.commentsService
        .getPostComments(this.post.postId, this.pageNumber, this.pageSize)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            const loadedComments = response.result;
            if (loadedComments) {
              const reversedComments = loadedComments.reverse();

              this.comments = [...reversedComments, ...this.comments];
              this.totalComments = response.pagination?.totalItems;
              if (
                !response.pagination ||
                loadedComments.length < this.pageSize
              ) {
                this.allCommentsLoaded = true;
              }
            }
          },
        });
    }
  }

  showMoreComments(): void {
    if (!this.allCommentsLoaded) {
      this.pageNumber++;
      this.loadComments();
    }
  }

  onCommentUpload(comment: any): void {
    this.comments = [...this.comments, comment.commentDto];
    this.post!.commentsCount += 1;
  }

  onCommentDelete(commentId: number): void {
    this.comments = this.comments.filter(
      (comment) => comment.commentId !== commentId
    );
    this.post!.commentsCount -= 1;
  }

  ngOnDestroy(): void {
    this.clickOutsideService.unbind(this);
  }
}
