import { Pipe, PipeTransform } from '@angular/core';
import { environment } from 'src/environments/environment';

@Pipe({
  name: 'relativeUrl',
  standalone: true,
})
//This helps us to get the relative url of the image, put your own url in the environment file with your cdn url
export class RelativeUrlPipe implements PipeTransform {
  private baseUrl: string = environment.cloudinaryUrl;

  transform(absoluteUrl: string): string {
    if (absoluteUrl.startsWith(this.baseUrl)) {
      return absoluteUrl.substring(
        this.baseUrl.length + '/image/upload/'.length
      );
    }
    return absoluteUrl;
  }
}
