import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
} from '@angular/core';
import { CommentsService } from 'src/app/shared/services/comments.service';
import { LikesService } from 'src/app/shared/services/likes.service';
import { Comment } from 'src/app/shared/models/comments';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { TimeAgoPipe } from '../../pipes/time-ago.pipe';
import { UserAvatarComponent } from '../user-avatar/user-avatar.component';
import { NgIf, NgClass } from '@angular/common';
import { Subject, first, takeUntil } from 'rxjs';
import { ClickOutsideService } from '../../services/click-outside.service';
import { LikedUser } from '../../models/likedUser';
import { LikedUsersListComponent } from '../liked-users-list/liked-users-list.component';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss'],
  standalone: true,
  imports: [
    NgIf,
    UserAvatarComponent,
    NgClass,
    TimeAgoPipe,
    LikedUsersListComponent,
  ],
})
export class CommentComponent implements OnDestroy {
  @Input() comment: Comment | undefined;

  @Output() commentDeleted: EventEmitter<number> = new EventEmitter();

  likedUsers: LikedUser[] = [];
  showLikedUsers: boolean = false;
  private destroy$ = new Subject<void>();

  constructor(
    private commentsService: CommentsService,
    private likesService: LikesService,
    public dialog: MatDialog,
    private clickOutsideService: ClickOutsideService
  ) {}

  toggleLike(comment: Comment) {
    if (comment.isLikedByCurrentUser) {
      this.likesService
        .unlikeComment(comment.commentId)
        .pipe(first())
        .subscribe(() => {
          comment.isLikedByCurrentUser = false;
          comment.likesCount -= 1;
        });
    } else {
      this.likesService
        .likeComment(comment.commentId)
        .pipe(first())
        .subscribe(() => {
          comment.isLikedByCurrentUser = true;
          comment.likesCount += 1;
        });
    }
  }

  displayLikedUsers(): void {
    if (this.comment?.likesCount === 0) {
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
    if (this.comment?.commentId) {
      this.likesService
        .getCommentLikes(this.comment.commentId)
        .pipe(takeUntil(this.destroy$))
        .subscribe((likedUsers) => {
          this.likedUsers = likedUsers;
        });
    }
  }

  deleteComment(commentId: number) {
    const dialogRef = this.dialog.open(DialogComponent, {
      data: {
        title: 'Delete Comment',
        message: 'Are you sure you want to delete this comment?',
      },
    });

    dialogRef
      .afterClosed()
      .pipe(first())
      .subscribe((result) => {
        if (result) {
          this.commentsService.deleteComment(commentId).subscribe(() => {
            this.comment = undefined;
            this.commentDeleted.emit(commentId);
          });
        }
      });
  }

  ngOnDestroy(): void {
    this.clickOutsideService.unbind(this);
  }
}
