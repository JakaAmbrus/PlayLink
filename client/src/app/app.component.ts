import { Component, OnDestroy, OnInit } from '@angular/core';
import { slideInAnimation } from './core/animations/route-animations';
import { Router, NavigationEnd, RouterOutlet } from '@angular/router';
import { PresenceService } from './core/services/presence.service';
import { HeaderComponent } from './core/components/header/header.component';
import { TokenService } from './core/services/token.service';
import { LocalStorageService } from './core/services/local-storage.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [slideInAnimation],
  standalone: true,
  imports: [HeaderComponent, RouterOutlet],
})
export class AppComponent implements OnInit, OnDestroy {
  private routerSubscription: Subscription;
  theme: 'theme-light' | 'theme-dark' = 'theme-light';
  showNavbar = true;

  constructor(
    private presenceService: PresenceService,
    private router: Router,
    private tokenService: TokenService,
    private localStorageService: LocalStorageService
  ) {
    this.routerSubscription = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.showNavbar =
          event.urlAfterRedirects !== '/portal' &&
          event.urlAfterRedirects !== '/not-found';
      }
    });
  }

  ngOnInit(): void {
    const token = this.tokenService.getToken();
    if (token) {
      this.presenceService.createHubConnection(token);
    } else {
      this.localStorageService.clearStorage();
      return;
    }

    const savedTheme = this.localStorageService.getItem('userThemePreference');
    if (savedTheme) {
      this.theme = savedTheme as 'theme-light' | 'theme-dark';
    } else {
      this.theme = 'theme-light';
    }
  }

  toggleTheme() {
    this.theme = this.theme === 'theme-light' ? 'theme-dark' : 'theme-light';
    this.localStorageService.setItem('userThemePreference', this.theme);
  }

  prepareRoute(outlet: RouterOutlet) {
    return (
      outlet &&
      outlet.activatedRouteData &&
      outlet.activatedRouteData['animation']
    );
  }

  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }
}
