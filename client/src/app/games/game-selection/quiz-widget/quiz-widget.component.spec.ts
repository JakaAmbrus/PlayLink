import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuizWidgetComponent } from './quiz-widget.component';

describe('QuizWidgetComponent', () => {
  let component: QuizWidgetComponent;
  let fixture: ComponentFixture<QuizWidgetComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [QuizWidgetComponent]
    });
    fixture = TestBed.createComponent(QuizWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
