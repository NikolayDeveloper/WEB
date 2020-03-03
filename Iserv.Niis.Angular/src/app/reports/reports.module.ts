import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'app/shared/shared.module';

import { ReportsComponent } from './reports.component';
import { ReportsRoutingModule } from 'app/reports/reports-routing.module';
import { ReportsService } from 'app/reports/reports.service';

import {ChartModule} from 'primeng/components/chart/chart';
import { ReportFilterComponent } from './components/report-filter/report-filter.component';
import { ChargedPaymentInvoicesReportFilterComponent } from './components/charged-paymnet-invoices-report-filter/charged-paymnet-invoices-report-filter.component';

@NgModule({
  imports: [
    SharedModule,
    ReportsRoutingModule,
    ChartModule,
  ],
  declarations: [
    ReportsComponent,
    ReportFilterComponent,
    ChargedPaymentInvoicesReportFilterComponent,
  ],
  providers: [
    ReportsService
  ]
})
export class ReportsModule { }
