import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GamesComponent } from '../features/games/games.component';

describe('GamesComponent', () => {
  let component: GamesComponent;
  let fixture: ComponentFixture<GamesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [GamesComponent],
});
    fixture = TestBed.createComponent(GamesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
