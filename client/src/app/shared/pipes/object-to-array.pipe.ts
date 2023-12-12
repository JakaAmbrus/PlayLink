import { Pipe, PipeTransform } from '@angular/core';
import { Message } from '../models/message';

@Pipe({
  name: 'objectToArray',
  standalone: true,
})
export class ObjectToArrayPipe implements PipeTransform {
  transform(value: { [key: string]: Message } | null): Message[] {
    if (!value) {
      return [];
    }
    return Object.values(value);
  }
}
