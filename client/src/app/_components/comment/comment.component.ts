import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommentsService } from 'src/app/_services/comments.service';
import { LikesService } from 'src/app/_services/likes.service';
import { Comment } from 'src/app/_models/comments';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss'],
})
export class CommentComponent {
  @Input() comment: Comment | undefined;

  @Output() commentDeleted: EventEmitter<number> = new EventEmitter();

  constructor(
    private commentsService: CommentsService,
    private likesService: LikesService
  ) {}

  toggleLike(comment: Comment) {
    if (comment.isLikedByCurrentUser) {
      this.likesService.unlikeComment(comment.commentId).subscribe(() => {
        comment.isLikedByCurrentUser = false;
        comment.likesCount -= 1;
      });
    } else {
      this.likesService.likeComment(comment.commentId).subscribe(() => {
        comment.isLikedByCurrentUser = true;
        comment.likesCount += 1;
      });
    }
  }

  deleteComment(commentId: number) {
    if (confirm('Are you sure you want to delete this comment?')) {
      this.commentsService.deleteComment(commentId).subscribe(() => {
        this.comment = undefined;
        this.commentDeleted.emit(commentId);
      });
    }
  }
}
