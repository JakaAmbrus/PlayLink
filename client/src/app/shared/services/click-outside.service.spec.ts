import { TestBed } from '@angular/core/testing';

import { ClickOutsideService } from './click-outside.service';

describe('ClickOutsideService', () => {
  let service: ClickOutsideService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClickOutsideService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
