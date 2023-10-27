import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GamesComponent } from './games.component';

describe('GamesComponent', () => {
  let component: GamesComponent;
  let fixture: ComponentFixture<GamesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GamesComponent]
    });
    fixture = TestBed.createComponent(GamesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
