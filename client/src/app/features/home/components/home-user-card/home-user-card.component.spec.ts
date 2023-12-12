import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeUserCardComponent } from './home-user-card.component';

describe('HomeUserCardComponent', () => {
  let component: HomeUserCardComponent;
  let fixture: ComponentFixture<HomeUserCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeUserCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HomeUserCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
