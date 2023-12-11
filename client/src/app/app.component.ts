import { Component, OnInit } from '@angular/core';
import { slideInAnimation } from './route-animations';
import { Router, NavigationEnd, RouterOutlet } from '@angular/router';
import { PresenceService } from './shared/services/presence.service';
import { HeaderComponent } from './core/components/header/header.component';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    animations: [slideInAnimation],
    standalone: true,
    imports: [
        NgIf,
        HeaderComponent,
        RouterOutlet,
    ],
})
export class AppComponent implements OnInit {
  theme: 'theme-light' | 'theme-dark' = 'theme-light';
  showNavbar = true;

  constructor(
    private presenceService: PresenceService,
    private router: Router
  ) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.showNavbar =
          event.urlAfterRedirects !== '/portal' &&
          event.urlAfterRedirects !== '/not-found';
      }
    });
  }

  ngOnInit(): void {
    const savedTheme = localStorage.getItem('userThemePreference');
    if (savedTheme) {
      this.theme = savedTheme as 'theme-light' | 'theme-dark';
    } else {
      this.theme = 'theme-light';
    }
    const token = localStorage.getItem('token');
    if (token) {
      this.presenceService.createHubConnection(token);
    }
  }

  toggleTheme() {
    this.theme = this.theme === 'theme-light' ? 'theme-dark' : 'theme-light';
    localStorage.setItem('userThemePreference', this.theme);
  }

  prepareRoute(outlet: any) {
    return (
      outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation
    );
  }
}
