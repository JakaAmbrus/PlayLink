import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-playsketch-portable',
  templateUrl: './playsketch-portable.component.html',
  styleUrls: ['./playsketch-portable.component.scss'],
})
export class PlaysketchPortableComponent {
  showIframe = false;

  ngOnInit(): void {
    this.showIframe = true;
  }

  ngOnDestroy(): void {
    this.showIframe = false;
  }
}
