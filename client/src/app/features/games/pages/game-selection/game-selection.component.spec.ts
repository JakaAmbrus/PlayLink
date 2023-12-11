import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameSelectionComponent } from '../pages/game-selection/game-selection.component';

describe('GameSelectionComponent', () => {
  let component: GameSelectionComponent;
  let fixture: ComponentFixture<GameSelectionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [GameSelectionComponent],
});
    fixture = TestBed.createComponent(GameSelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
