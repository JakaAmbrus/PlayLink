import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-hollow-x-hollow',
  templateUrl: './hollow-x-hollow.component.html',
  styleUrls: ['./hollow-x-hollow.component.scss'],
})
export class HollowXHollowComponent {
  showIframe = false;

  ngOnInit(): void {
    this.showIframe = true;
  }

  ngOnDestroy(): void {
    this.showIframe = false;
  }
}
