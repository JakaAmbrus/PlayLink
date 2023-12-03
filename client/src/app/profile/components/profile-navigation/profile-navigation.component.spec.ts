import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileNavigationComponent } from './profile-navigation.component';

describe('ProfileNavigationComponent', () => {
  let component: ProfileNavigationComponent;
  let fixture: ComponentFixture<ProfileNavigationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileNavigationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProfileNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
