import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RockPaperScissorsComponent } from './rock-paper-scissors.component';

describe('RockPaperScissorsComponent', () => {
  let component: RockPaperScissorsComponent;
  let fixture: ComponentFixture<RockPaperScissorsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [RockPaperScissorsComponent]
});
    fixture = TestBed.createComponent(RockPaperScissorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
