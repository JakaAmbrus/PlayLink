import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileUserCardComponent } from './profile-user-card.component';

describe('ProfileUserCardComponent', () => {
  let component: ProfileUserCardComponent;
  let fixture: ComponentFixture<ProfileUserCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileUserCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProfileUserCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
