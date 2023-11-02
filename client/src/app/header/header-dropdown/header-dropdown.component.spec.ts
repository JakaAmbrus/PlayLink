import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderDropdownComponent } from './header-dropdown.component';

describe('HeaderDropdownComponent', () => {
  let component: HeaderDropdownComponent;
  let fixture: ComponentFixture<HeaderDropdownComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HeaderDropdownComponent]
    });
    fixture = TestBed.createComponent(HeaderDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
