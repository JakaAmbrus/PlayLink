import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'limitText',
})
export class LimitTextPipe implements PipeTransform {
  transform(value: string, maxLength: number = 20): string {
    if (!value) return value;
    if (value.length <= maxLength) return value;

    return value.substr(0, maxLength) + '...';
  }
}
