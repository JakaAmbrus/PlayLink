import { TestBed } from '@angular/core/testing';

import { MessageDisplayService } from './message-display.service';

describe('MessageDisplayService', () => {
  let service: MessageDisplayService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MessageDisplayService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
