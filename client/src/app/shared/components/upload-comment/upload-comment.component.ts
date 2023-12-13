import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Avatar } from 'src/app/shared/models/avatar';
import { AvatarService } from 'src/app/shared/services/avatar.service';
import { CommentsService } from 'src/app/shared/services/comments.service';
import { NgIf } from '@angular/common';
import { UserAvatarComponent } from '../user-avatar/user-avatar.component';
import { first } from 'rxjs';

@Component({
  selector: 'app-upload-comment',
  templateUrl: './upload-comment.component.html',
  styleUrls: ['./upload-comment.component.scss'],
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, UserAvatarComponent, NgIf],
})
export class UploadCommentComponent implements OnInit {
  @Output() commentUploaded: EventEmitter<any> = new EventEmitter();

  @Input() postId: number | undefined;

  uploadCommentForm: FormGroup = new FormGroup({});
  avatar!: Avatar;

  constructor(
    private fb: FormBuilder,
    private commentsService: CommentsService,
    private avatarService: AvatarService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.avatar = this.avatarService.getAvatarDetails();
  }

  initializeForm() {
    this.uploadCommentForm = this.fb.group({
      content: ['', [Validators.required, Validators.maxLength(400)]],
    });
  }

  onSubmit() {
    if (this.uploadCommentForm.valid && this.postId) {
      const commentContent = this.uploadCommentForm.get('content')?.value;
      const commentUploadDto = {
        postId: this.postId,
        content: commentContent,
      };
      this.commentsService
        .uploadComment(commentUploadDto)
        .pipe(first())
        .subscribe({
          next: (response) => {
            this.commentUploaded.emit(response);
            this.uploadCommentForm.reset();
          },
          error: (err) => console.error(err),
        });
    }
  }
}
