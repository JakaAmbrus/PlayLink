import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  standalone: true,
  name: 'relativeTime',
})
export class RelativeTimePipe implements PipeTransform {
  transform(date: Date | string): string {
    if (date) {
      const currentDate = new Date();
      const inputDate = new Date(date);
      const seconds = Math.floor(
        (currentDate.getTime() - inputDate.getTime()) / 1000
      );
      if (seconds < 0) {
        return `0 seconds ago`;
      } else if (seconds < 60) {
        return `${seconds} seconds ago`;
      } else if (seconds < 3600) {
        return `${Math.floor(seconds / 60)} minutes ago`;
      } else if (seconds < 86400) {
        return `${Math.floor(seconds / 3600)} hours ago`;
      } else {
        return inputDate.toLocaleDateString();
      }
    }
    return '';
  }
}
