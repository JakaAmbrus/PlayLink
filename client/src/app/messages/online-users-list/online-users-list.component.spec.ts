import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OnlineUsersListComponent } from './online-users-list.component';

describe('OnlineUsersListComponent', () => {
  let component: OnlineUsersListComponent;
  let fixture: ComponentFixture<OnlineUsersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OnlineUsersListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OnlineUsersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
