import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'firstWord',
})
export class FirstWordPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return value;
    let words = value.split(' ');
    return words[0];
  }
}
