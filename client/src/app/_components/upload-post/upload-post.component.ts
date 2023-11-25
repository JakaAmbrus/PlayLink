import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-upload-post',
  templateUrl: './upload-post.component.html',
  styleUrls: ['./upload-post.component.scss'],
})
export class UploadPostComponent implements OnInit {
  isModalOpen: boolean = false;
  uploadPostForm: FormGroup = new FormGroup({});
  selectedFiles: File[] = [];
  isFileSelected: boolean = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.uploadPostForm = this.fb.group({
      description: ['', [Validators.maxLength(200)]],
    });
  }

  openUploadPostModal() {
    this.isModalOpen = true;
  }

  onSelect(event: any) {
    this.selectedFiles = event.addedFiles.slice(0, 1);
    this.isFileSelected = true;
  }

  onRemove(event: any) {
    this.selectedFiles = [];
    this.isFileSelected = false;
  }

  onSubmit() {}
}
