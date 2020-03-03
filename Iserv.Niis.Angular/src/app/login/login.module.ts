import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '../shared/shared.module';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { DialogForPasswordComponent } from './dialog/dialog-for-password.component';
import { AdministrationModule } from 'app/administration/administration.module';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    LoginRoutingModule,
    AdministrationModule
  ],
  declarations: [LoginComponent, DialogForPasswordComponent],
  entryComponents: [DialogForPasswordComponent]
})
export class LoginModule { }
