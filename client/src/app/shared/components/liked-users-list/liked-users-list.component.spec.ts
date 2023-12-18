import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LikedUsersListComponent } from './liked-users-list.component';

describe('LikedUsersListComponent', () => {
  let component: LikedUsersListComponent;
  let fixture: ComponentFixture<LikedUsersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LikedUsersListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LikedUsersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
