import { Component, Output, OnInit, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  model: any = {};

  @Output() exitRegistration = new EventEmitter<void>();

  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {}

  register() {
    console.log(this.model);
    const registerModel = {
      username: this.model.username,
      password: this.model.password,
    };
    this.accountService.register(registerModel).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigate(['/portal']);
        this.exitRegistration.emit();
      },
      error: (error) => console.log(error),
    });
  }

  cancel(): void {
    console.log('cancelled');
    this.exitRegistration.emit();
  }
}
