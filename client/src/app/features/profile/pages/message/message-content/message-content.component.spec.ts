import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageContentComponent } from './message-content.component';

describe('MessageContentComponent', () => {
  let component: MessageContentComponent;
  let fixture: ComponentFixture<MessageContentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MessageContentComponent]
    });
    fixture = TestBed.createComponent(MessageContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
