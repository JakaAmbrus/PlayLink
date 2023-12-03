import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { slideInAnimation } from './route-animations';
import { Router, NavigationEnd } from '@angular/router';
import { PresenceService } from './_services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [slideInAnimation],
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
        this.showNavbar = event.urlAfterRedirects !== '/portal';
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
