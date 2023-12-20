import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuestLoginDialogComponent } from './guest-login-dialog.component';

describe('GuestLoginDialogComponent', () => {
  let component: GuestLoginDialogComponent;
  let fixture: ComponentFixture<GuestLoginDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuestLoginDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GuestLoginDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
