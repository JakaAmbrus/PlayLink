import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PostContent } from 'src/app/_models/posts';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-upload-post',
  templateUrl: './upload-post.component.html',
  styleUrls: ['./upload-post.component.scss'],
  animations: [
    trigger('growShrink', [
      state(
        'void',
        style({
          transform: 'scale(0)',
          opacity: 0,
          display: 'none',
        })
      ),
      state(
        'small',
        style({
          transform: 'scale(0)',
          opacity: 0,
          display: 'none',
        })
      ),
      state(
        'large',
        style({
          transform: 'scale(1)',
          opacity: 1,
          display: 'flex',
        })
      ),
      transition('void <=> large', animate('100ms ease-out')),
      transition('small <=> large', animate('100ms ease-in')),
    ]),
  ],
})
export class UploadPostComponent implements OnInit {
  @Output() postUploaded: EventEmitter<any> = new EventEmitter();

  animationState = 'small';
  isModalOpen: boolean = false;
  uploadPostForm: FormGroup = new FormGroup({});
  selectedFiles: File[] = [];
  isFileSelected: boolean = false;
  isLoading: boolean = false;

  constructor(private fb: FormBuilder, private postsService: PostsService) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.uploadPostForm = this.fb.group({
      description: ['', [Validators.maxLength(300)]],
    });
  }

  openUploadPostModal() {
    this.isModalOpen = true;
    this.animationState = 'large';
  }

  closeUploadPostModal() {
    this.animationState = 'small';
    setTimeout(() => {
      this.isModalOpen = false;
    }, 105);
  }

  onSelect(event: any) {
    this.selectedFiles = event.addedFiles.slice(0, 1);
    this.isFileSelected = true;
  }

  onRemove(event: any) {
    this.selectedFiles = [];
    this.isFileSelected = false;
  }

  onSubmit(): void {
    if (this.uploadPostForm.valid || this.isFileSelected) {
      this.isLoading = true;

      const postContentData: PostContent = {
        description: this.uploadPostForm.value.description,
        photoFile: this.selectedFiles[0],
      };
      this.postsService.uploadPost(postContentData).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.postUploaded.emit(response.postDto);
          this.closeUploadPostModal();
          this.uploadPostForm.reset();
          this.selectedFiles = [];
          this.isFileSelected = false;
        },
        error: () => {
          this.isLoading = false;
        },
      });
    }
  }
}
