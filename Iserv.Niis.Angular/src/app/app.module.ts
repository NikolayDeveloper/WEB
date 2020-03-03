import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxPermissionsModule } from 'ngx-permissions';

import { AccessDeniedComponent } from './access-denied.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConfigService } from './core';
import { CoreModule } from './core/core.module';
import { ErrorHandlerService } from './core/error-handler.service';
import { SnackBarHelper } from './core/snack-bar-helper.service';
import { PageNotFoundComponent } from './page-not-found.component';
import { SharedModule } from './shared/shared.module';

import { ReportsModule } from './reports/reports.module';
import { PaymentsJournalModule } from './payments-journal/payments-journal.module';
import { SystemService } from './shared/services/system.service';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    AccessDeniedComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    NgxPermissionsModule.forRoot(),
    SharedModule,
    CoreModule,
    AppRoutingModule,
    ReportsModule,
    PaymentsJournalModule
  ],
  providers: [
    ErrorHandlerService,
    ConfigService,
    SnackBarHelper,
    SystemService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor() { }
}
