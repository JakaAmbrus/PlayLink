import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OnlineUserDisplayComponent } from './online-user-display.component';

describe('OnlineUserDisplayComponent', () => {
  let component: OnlineUserDisplayComponent;
  let fixture: ComponentFixture<OnlineUserDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OnlineUserDisplayComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OnlineUserDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
