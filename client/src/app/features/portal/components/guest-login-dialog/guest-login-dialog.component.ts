import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-guest-login-dialog',
  standalone: true,
  templateUrl: './guest-login-dialog.component.html',
  styleUrl: './guest-login-dialog.component.scss',
})
export class GuestLoginDialogComponent {
  constructor(private dialogRef: MatDialogRef<GuestLoginDialogComponent>) {}

  onSelect(role: string): void {
    this.dialogRef.close(role);
  }
}
