import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'relativeUrl',
  standalone: true,
})
export class RelativeUrlPipe implements PipeTransform {
  transform(absoluteUrl: string, baseUrl: string): string {
    if (absoluteUrl.startsWith(baseUrl)) {
      return absoluteUrl.substring(baseUrl.length + '/image/upload/'.length);
    }
    return absoluteUrl;
  }
}
