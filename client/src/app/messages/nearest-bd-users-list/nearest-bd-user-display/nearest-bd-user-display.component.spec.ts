import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NearestBdUserDisplayComponent } from './nearest-bd-user-display.component';

describe('NearestBdUserDisplayComponent', () => {
  let component: NearestBdUserDisplayComponent;
  let fixture: ComponentFixture<NearestBdUserDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NearestBdUserDisplayComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NearestBdUserDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
