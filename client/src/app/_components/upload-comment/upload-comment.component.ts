import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommentsService } from 'src/app/_services/comments.service';

@Component({
  selector: 'app-upload-comment',
  templateUrl: './upload-comment.component.html',
  styleUrls: ['./upload-comment.component.scss'],
})
export class UploadCommentComponent implements OnInit {
  @Output() commentUploaded: EventEmitter<any> = new EventEmitter();

  @Input() postId: number | undefined;

  uploadCommentForm: FormGroup = new FormGroup({});

  constructor(
    private fb: FormBuilder,
    private commentsService: CommentsService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.uploadCommentForm = this.fb.group({
      content: ['', [Validators.required, Validators.maxLength(400)]],
    });
  }

  onSubmit() {
    if (this.uploadCommentForm.valid && this.postId) {
      const commentContent = this.uploadCommentForm.get('content')?.value;
      this.commentsService
        .uploadComment(this.postId, commentContent)
        .subscribe({
          next: (response) => {
            console.log(response);
            this.commentUploaded.emit(response);
            this.uploadCommentForm.reset();
          },
          error: (err) => console.error(err),
        });
    }
  }
}
