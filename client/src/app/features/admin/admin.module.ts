import { NgModule } from '@angular/core';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from 'src/app/features/admin/admin.component';
import { AdminUserDisplayComponent } from './components/admin-user-display/admin-user-display.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  declarations: [AdminComponent, AdminUserDisplayComponent],
  imports: [AdminRoutingModule, SharedModule, NgxPaginationModule],
})
export class AdminModule {
  constructor() {
    console.log('Admin Module Loaded');
  }
}
