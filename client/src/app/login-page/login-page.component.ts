import { Component } from '@angular/core';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent {}
// users: any;

// constructor(private http: HttpClient) {}
// ngOnInit(): void {
//   this.http.get('https://localhost:7074/api/users').subscribe({
//     next: (data) => {
//       this.users = data;
//     },
//     error: (err) => {
//       console.error('There was an error!', err);
//     },
//     complete: () => {
//       console.log('Request completed');
//     },
//   });
// }
