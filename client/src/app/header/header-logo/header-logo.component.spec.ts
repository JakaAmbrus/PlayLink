import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderLogoComponent } from './header-logo.component';

describe('HeaderLogoComponent', () => {
  let component: HeaderLogoComponent;
  let fixture: ComponentFixture<HeaderLogoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HeaderLogoComponent]
    });
    fixture = TestBed.createComponent(HeaderLogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
