import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaysketchPortableComponent } from './playsketch-portable.component';

describe('PlaysketchPortableComponent', () => {
  let component: PlaysketchPortableComponent;
  let fixture: ComponentFixture<PlaysketchPortableComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [PlaysketchPortableComponent]
});
    fixture = TestBed.createComponent(PlaysketchPortableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
