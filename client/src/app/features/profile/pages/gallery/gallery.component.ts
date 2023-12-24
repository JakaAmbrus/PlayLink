import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { first } from 'rxjs';
import { UserPhotosService } from '../../services/user-photos.service';

@Component({
  selector: 'app-gallery',
  standalone: true,
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss'],
  imports: [GalleryModule, CommonModule],
})
export class GalleryComponent implements OnInit {
  postPhotos: GalleryItem[] = [];
  username: any;
  noPhotos: boolean = false;

  constructor(
    private userPhotosService: UserPhotosService,
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

    this.userPhotosService
      .getUserPhotos(this.username)
      .pipe(first())
      .subscribe((photos) => {
        this.postPhotos = this.transformPhotosToGalleryItems(photos);
        if (this.postPhotos.length === 0) {
          this.noPhotos = true;
        }
      });
  }

  transformPhotosToGalleryItems(photos: string[]): GalleryItem[] {
    return photos.map(
      (photoUrl) => new ImageItem({ src: photoUrl, thumb: photoUrl })
    );
  }
}
