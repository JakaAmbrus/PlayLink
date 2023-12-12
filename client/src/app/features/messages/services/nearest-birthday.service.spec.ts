import { TestBed } from '@angular/core/testing';

import { NearestBirthdayService } from './nearest-birthday.service';

describe('NearestBirthdayService', () => {
  let service: NearestBirthdayService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NearestBirthdayService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
