import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommentsService } from 'src/app/shared/services/comments.service';
import { LikesService } from 'src/app/shared/services/likes.service';
import { Comment } from 'src/app/shared/models/comments';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { TimeAgoPipe } from '../../pipes/time-ago.pipe';
import { UserAvatarComponent } from '../user-avatar/user-avatar.component';
import { NgIf, NgClass } from '@angular/common';
import { first } from 'rxjs';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss'],
  standalone: true,
  imports: [NgIf, UserAvatarComponent, NgClass, TimeAgoPipe],
})
export class CommentComponent {
  @Input() comment: Comment | undefined;

  @Output() commentDeleted: EventEmitter<number> = new EventEmitter();

  constructor(
    private commentsService: CommentsService,
    private likesService: LikesService,
    public dialog: MatDialog
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
}
