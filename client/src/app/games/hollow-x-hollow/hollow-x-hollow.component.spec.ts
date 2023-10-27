import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HollowXHollowComponent } from './hollow-x-hollow.component';

describe('HollowXHollowComponent', () => {
  let component: HollowXHollowComponent;
  let fixture: ComponentFixture<HollowXHollowComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HollowXHollowComponent]
    });
    fixture = TestBed.createComponent(HollowXHollowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
