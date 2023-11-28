import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-gallery',
  standalone: true,
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss'],
  imports: [GalleryModule],
})
export class GalleryComponent implements OnInit {
  postPhotos: GalleryItem[] = [];
  username: any;

  constructor(
    private postsService: PostsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadPhotos();
  }

  loadPhotos(): void {
    this.username = this.route.parent?.snapshot.paramMap.get('username');
    if (!this.username) {
      return;
    }

    this.postsService.getUserPostPhotos(this.username).subscribe((photos) => {
      this.postPhotos = this.transformPhotosToGalleryItems(photos);
    });
  }

  transformPhotosToGalleryItems(photos: string[]): GalleryItem[] {
    return photos.map(
      (photoUrl) => new ImageItem({ src: photoUrl, thumb: photoUrl })
    );
  }
}
