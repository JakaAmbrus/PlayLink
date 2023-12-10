import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NearestBdUsersListComponent } from './nearest-bd-users-list.component';

describe('NearestBdUsersListComponent', () => {
  let component: NearestBdUsersListComponent;
  let fixture: ComponentFixture<NearestBdUsersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NearestBdUsersListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NearestBdUsersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
