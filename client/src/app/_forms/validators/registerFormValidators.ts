import { AbstractControl, ValidatorFn } from '@angular/forms';
import countries from '../../../assets/data/countries.json';

export function standardLettersOnlyValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isNotLettersOnly = /[^A-Za-z]/.test(control.value);
    return isNotLettersOnly ? { lettersOnly: { value: control.value } } : null;
  };
}

export function standardLettersAndSpacesValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isNotLettersOrSpace = /[^A-Za-z\s]/.test(control.value);
    return isNotLettersOrSpace
      ? { lettersAndSpacesOnly: { value: control.value } }
      : null;
  };
}

export function atLeastOneNumberValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const hasNumber = /[0-9]/.test(control.value);
    return hasNumber ? null : { atLeastOneNumber: { value: control.value } };
  };
}

export function hasSpaceValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const hasSpace = /\s/.test(control.value);
    return hasSpace ? null : { noSpace: true };
  };
}

export function matchValues(matchTo: string): ValidatorFn {
  return (control: AbstractControl) => {
    return control.value === control.parent?.get(matchTo)?.value
      ? null
      : { isMatching: true };
  };
}

export function validCountryValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    if (!control.value) {
      return null;
    }
    const isValid = countries.includes(control.value);
    return isValid ? null : { invalidCountry: true };
  };
}
