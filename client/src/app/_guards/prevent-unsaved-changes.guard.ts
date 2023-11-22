import { CanDeactivateFn } from '@angular/router';
import { EditComponent } from '../profile/edit/edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<EditComponent> = (
  component
) => {
  if (component.editUserForm?.dirty) {
    return confirm(
      'Are you sure you want to continue? Any unsaved changes will be lost.'
    );
  }
  return true;
};
