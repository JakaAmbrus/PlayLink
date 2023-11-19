import { Component } from '@angular/core';
import { GalleryItem, GalleryModule } from 'ng-gallery';

@Component({
  selector: 'app-gallery',
  standalone: true,
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss'],
  imports: [GalleryModule],
})
export class GalleryComponent {
  postPhotos: GalleryItem[] = [];

  getPostPhotos() {}
}
