import { FirstWordPipe } from './first-word.pipe';

describe('FirstWordPipe', () => {
  it('create an instance', () => {
    const pipe = new FirstWordPipe();
    expect(pipe).toBeTruthy();
  });
});
