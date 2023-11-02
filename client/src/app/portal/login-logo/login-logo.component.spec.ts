import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginLogoComponent } from './login-logo.component';

describe('LoginLogoComponent', () => {
  let component: LoginLogoComponent;
  let fixture: ComponentFixture<LoginLogoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LoginLogoComponent]
    });
    fixture = TestBed.createComponent(LoginLogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
