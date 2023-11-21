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
  validCountryValidator,
} from 'src/app/_forms/validators/registerFormValidators';
import { Observable, debounceTime, map, startWith } from 'rxjs';
import countries from '../../../../assets/data/countries.json';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  model: any = {};
  registerForm: FormGroup = new FormGroup({});
  minDate: Date;
  maxDate: Date;
  filteredCountries?: Observable<string[]>;

  @Output() exitRegistration = new EventEmitter<void>();

  constructor(
    private accountService: AccountService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.minDate = new Date(1925, 0, 1);
    this.maxDate = new Date(2010, 5, 30);
  }

  ngOnInit(): void {
    this.initializeForm();

    this.filteredCountries = this.registerForm.controls[
      'country'
    ].valueChanges.pipe(
      debounceTime(100),
      startWith(''),
      map((value) => (typeof value === 'string' ? value : value.name)),
      map((name) => (name ? this._filterCountries(name) : countries.slice()))
    );
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

      dateOfBirth: [
        '',
        [Validators.required, Validators.max(1925), Validators.min(2012)],
      ],

      country: ['', [Validators.required, validCountryValidator()]],

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

  displayFn(country: string): string {
    return country ? country : '';
  }

  private _filterCountries(value: string): string[] {
    const filterValue = value.toLowerCase();
    return countries.filter((country) =>
      country.toLowerCase().includes(filterValue)
    );
  }

  register() {
    const dob = this.getOnlyDate(
      this.registerForm?.controls['dateOfBirth'].value
    );
    const values = { ...this.registerForm.value, dateOfBirth: dob };

    this.accountService.register(values).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigate(['/portal']);
        this.exitRegistration.emit();
      },
      error: () => console.log('Registration error'),
    });
  }

  cancel(): void {
    console.log('cancelled');
    this.exitRegistration.emit();
  }

  private getOnlyDate(dob: string | undefined) {
    if (!dob) return;

    let date = new Date(dob);

    return new Date(
      date.setMinutes(date.getMinutes() - date.getTimezoneOffset())
    )
      .toISOString()
      .slice(0, 10);
  }
}
