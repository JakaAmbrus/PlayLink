import {
  ChangeDetectorRef,
  Component,
  Host,
  HostListener,
  OnInit,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import countries from '../../../assets/data/countries.json';
import { Observable, debounceTime, map, startWith } from 'rxjs';
import {
  allOptionalFieldsEmptyValidator,
  validCountryValidator,
} from 'src/app/_forms/validators/registerFormValidators';
import { EditUser, EditUserResponse } from 'src/app/_models/users';
import { UsersService } from 'src/app/_services/users.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
})
export class EditComponent implements OnInit {
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.editUserForm.dirty) {
      $event.returnValue = true;
    }
  }

  username: any;
  editUserForm: FormGroup = new FormGroup({});
  selectedFiles: File[] = [];
  filteredCountries?: Observable<string[]>;
  isFileSelected: boolean = false;
  isLoading: boolean = false;

  constructor(
    private usersService: UsersService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private cdRef: ChangeDetectorRef
  ) {}

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
    this.editUserForm = this.fb.group({
      description: ['', [Validators.maxLength(200)]],
      country: ['', [validCountryValidator()]],
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

  onSelect(event: any) {
    this.selectedFiles = event.addedFiles.slice(0, 1);
    this.isFileSelected = true;
  }

  onRemove(event: any) {
    this.selectedFiles = [];
    this.isFileSelected = false;
  }

  onSubmit() {
    if (this.editUserForm.valid || this.isFileSelected) {
      this.isLoading = true;
      const formData = new FormData();

      const editUserData: EditUser = {
        username: this.username,
        image: this.selectedFiles.length > 0 ? this.selectedFiles[0] : null,
        description: this.editUserForm.get('description')?.value,
        country: this.editUserForm.get('country')?.value,
      };
      console.log(editUserData);

      this.usersService.editUser(editUserData).subscribe({
        next: () => {
          this.toastr.success('Profile updated successfully');
          this.editUserForm.reset();
          this.selectedFiles = [];
          this.cdRef.detectChanges();
          this.isLoading = false;
          this.router
            .navigateByUrl('/RefreshComponent', { skipLocationChange: true })
            .then(() => {
              this.router.navigate(['/user', this.username, 'edit']);
            });
        },
        error: (error) => {
          this.isLoading = false;
        },
      });
    }
  }
}
