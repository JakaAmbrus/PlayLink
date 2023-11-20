import { Component, Output, OnInit, EventEmitter } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';
import {
  atLeastOneNumberValidator,
  matchValues,
  hasSpaceValidator,
  standardLettersOnlyValidator,
  standardLettersAndSpacesValidator,
} from 'src/app/_forms/validators/registerFormValidators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  model: any = {};
  registerForm: FormGroup = new FormGroup({});

  @Output() exitRegistration = new EventEmitter<void>();

  constructor(
    private accountService: AccountService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.registerForm = this.fb.group({
      gender: ['male', Validators.required],

      username: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(10),
          standardLettersOnlyValidator(),
        ],
      ],

      fullName: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(30),
          standardLettersAndSpacesValidator(),
          hasSpaceValidator(),
        ],
      ],

      dateOfBirth: ['', [Validators.required]],

      country: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(20),
          standardLettersAndSpacesValidator(),
        ],
      ],

      city: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(20),
          standardLettersAndSpacesValidator(),
        ],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(20),
          atLeastOneNumberValidator(),
        ],
      ],
      confirmPassword: ['', [Validators.required, matchValues('password')]],
    });

    this.registerForm.controls['password'].valueChanges.subscribe(() => {
      this.registerForm.controls['confirmPassword'].updateValueAndValidity();
    });
  }

  register() {
    console.log(this.registerForm?.value);
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
