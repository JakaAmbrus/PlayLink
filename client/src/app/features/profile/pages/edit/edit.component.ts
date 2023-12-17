import {
  ChangeDetectorRef,
  Component,
  HostListener,
  OnInit,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import countries from '../../../../../assets/data/countries.json';
import { Observable, debounceTime, map, startWith } from 'rxjs';
import { validCountryValidator } from 'src/app/shared/validators/formValidators';
import { ToastrService } from 'ngx-toastr';
import { AvatarService } from 'src/app/shared/services/avatar.service';
import { MatOptionModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { NgFor, NgIf, AsyncPipe } from '@angular/common';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { EditUserService } from '../../services/edit-user.service';
import { EditUser } from '../../models/edit-user';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgxDropzoneModule,
    NgFor,
    NgIf,
    MatAutocompleteModule,
    MatOptionModule,
    AsyncPipe,
  ],
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
    private editUserService: EditUserService,
    private userProfileService: UserProfileService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private cdRef: ChangeDetectorRef,
    private avatarService: AvatarService
  ) {}

  ngOnInit(): void {
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

  onSelect(event: any): void {
    this.selectedFiles = event.addedFiles.slice(0, 1);
    this.isFileSelected = true;
  }

  onRemove(event: any): void {
    this.selectedFiles = [];
    this.isFileSelected = false;
  }

  onSubmit(): void {
    if (this.editUserForm.valid || this.isFileSelected) {
      this.isLoading = true;

      const editUserData: EditUser = {
        username: this.username,
        image: this.selectedFiles.length > 0 ? this.selectedFiles[0] : null,
        description: this.editUserForm.get('description')?.value,
        country: this.editUserForm.get('country')?.value,
      };

      this.editUserService.editUser(editUserData).subscribe({
        next: (response) => {
          this.toastr.success('Profile updated successfully');
          this.editUserForm.reset();
          this.selectedFiles = [];
          this.cdRef.detectChanges();
          this.isLoading = false;
          if (response.photoUrl) {
            this.avatarService.updateAvatarPhoto(response.photoUrl);
          }
          this.userProfileService.invalidateUserCache(this.username);
          this.router
            .navigateByUrl('/RefreshComponent', { skipLocationChange: true })
            .then(() => {
              this.router.navigate(['/user', this.username, 'edit']);
            });
        },
        error: () => {
          this.isLoading = false;
        },
      });
    }
  }
}
