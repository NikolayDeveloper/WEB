import { NgModule } from '@angular/core';
import { AUTH_SERVICE, AuthModule, PROTECTED_FALLBACK_PAGE_URI, PUBLIC_FALLBACK_PAGE_URI } from 'ngx-auth';

import { AuthenticationService } from './authentication.service';
import { PermissionConstantService } from './perpission-constant.service';
import { TokenStorageService } from './token-storage.service';

@NgModule({
  imports: [
    AuthModule
  ],
  providers: [
    TokenStorageService,
    AuthenticationService,
    PermissionConstantService,
    { provide: PROTECTED_FALLBACK_PAGE_URI, useValue: '/' },
    { provide: PUBLIC_FALLBACK_PAGE_URI, useValue: '/login' },
    {
      provide: AUTH_SERVICE, useClass: AuthenticationService
    }
  ],
  declarations: []
})
export class AuthenticationModule { }
