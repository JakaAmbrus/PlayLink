import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendListComponent } from '../../features/home/friend-list/friend-list.component';

describe('FriendListComponent', () => {
  let component: FriendListComponent;
  let fixture: ComponentFixture<FriendListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FriendListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FriendListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
