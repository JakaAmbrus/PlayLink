import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { first } from 'rxjs';
import { UserPhotosService } from '../../services/user-photos.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
  selector: 'app-gallery',
  standalone: true,
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss'],
  imports: [GalleryModule, SpinnerComponent],
})
export class GalleryComponent implements OnInit {
  loadingState: string = 'Loading';
  isLoading: boolean = false;
  loadingError: boolean = false;
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
    if (!this.username || this.isLoading) {
      return;
    }
    this.loadingState = 'Loading';

    this.userPhotosService
      .getUserPhotos(this.username)
      .pipe(first())
      .subscribe({
        next: (photos) => {
          this.postPhotos = this.transformPhotosToGalleryItems(photos);
          this.loadingState = photos.length === 0 ? 'NoPhotos' : 'Loaded';
        },
        error: () => {
          this.loadingState = 'Error';
        },
      });
  }

  transformPhotosToGalleryItems(photos: string[]): GalleryItem[] {
    return photos.map(
      (photoUrl) => new ImageItem({ src: photoUrl, thumb: photoUrl })
    );
  }
}
