import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { slideInAnimation } from './route-animations';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [slideInAnimation],
})
export class AppComponent implements OnInit {
  theme: 'theme-light' | 'theme-dark' = 'theme-light';

  toggleTheme() {
    this.theme = this.theme === 'theme-light' ? 'theme-dark' : 'theme-light';
    localStorage.setItem('userThemePreference', this.theme);
  }

  prepareRoute(outlet: any) {
    return (
      outlet && outlet.activatedRouteData && outlet.activatedRouteData.animation
    );
  }

  users: any;
  showNavbar = true;

  constructor(private http: HttpClient, private router: Router) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.showNavbar = event.urlAfterRedirects !== '/login';
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

    this.http.get('https://localhost:7074/api/users').subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {
        console.error('There was an error!', err);
      },
      complete: () => {
        console.log('Request completed');
      },
    });
  }
}
