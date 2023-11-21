import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import countries from '../../../assets/data/countries.json';
import { Observable, debounceTime, map, startWith } from 'rxjs';
import {
  allOptionalFieldsEmptyValidator,
  validCountryValidator,
} from 'src/app/_forms/validators/registerFormValidators';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
})
export class EditComponent implements OnInit {
  username: any;
  editUserForm: FormGroup = new FormGroup({});
  selectedFiles: File[] = [];
  filteredCountries?: Observable<string[]>;

  constructor(private fb: FormBuilder, private route: ActivatedRoute) {}

  ngOnInit() {
    this.initializeForm();

    this.username = this.route.parent?.snapshot.paramMap.get('username');

    this.filteredCountries = this.editUserForm.controls[
      'country'
    ].valueChanges.pipe(
      debounceTime(100),
      startWith(''),
      map((value) => (typeof value === 'string' ? value : value.name)),
      map((name) => (name ? this._filterCountries(name) : countries.slice()))
    );
  }

  initializeForm(): void {
    this.editUserForm = this.fb.group(
      {
        image: [''],
        description: ['', [Validators.maxLength(200)]],
        country: ['', [validCountryValidator()]],
      },
      { validators: allOptionalFieldsEmptyValidator }
    );
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

  onSelect(event: any) {
    this.selectedFiles = event.addedFiles.slice(0, 1);
  }

  onRemove(event: any) {
    this.selectedFiles = [];
  }

  onSubmit() {
    if (this.editUserForm.valid) {
      const formData = new FormData();
      formData.append('username', this.username);

      if (this.selectedFiles.length > 0) {
        formData.append(
          'image',
          this.selectedFiles[0],
          this.selectedFiles[0].name
        );
      }

      formData.append(
        'description',
        this.editUserForm.get('description')?.value
      );
      formData.append('country', this.editUserForm.get('country')?.value);

      // Now call your service method to make the HTTP request
      // this.editUserService.editUser(formData).subscribe(...);
    }
  }
}
