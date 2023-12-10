import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { adminGuard } from 'src/app/features/admin/guards/admin.guard';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [adminGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
