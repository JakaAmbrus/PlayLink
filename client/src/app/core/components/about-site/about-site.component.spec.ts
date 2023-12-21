import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutSiteComponent } from './about-site.component';

describe('AboutSiteComponent', () => {
  let component: AboutSiteComponent;
  let fixture: ComponentFixture<AboutSiteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AboutSiteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AboutSiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
