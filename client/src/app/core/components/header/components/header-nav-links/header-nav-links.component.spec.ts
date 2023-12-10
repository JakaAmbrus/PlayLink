import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderNavLinksComponent } from './header-nav-links.component';

describe('HeaderNavLinksComponent', () => {
  let component: HeaderNavLinksComponent;
  let fixture: ComponentFixture<HeaderNavLinksComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HeaderNavLinksComponent]
    });
    fixture = TestBed.createComponent(HeaderNavLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
