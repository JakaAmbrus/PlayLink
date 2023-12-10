import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadCommentComponent } from './upload-comment.component';

describe('UploadCommentComponent', () => {
  let component: UploadCommentComponent;
  let fixture: ComponentFixture<UploadCommentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UploadCommentComponent]
    });
    fixture = TestBed.createComponent(UploadCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
