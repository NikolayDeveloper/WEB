import { CdkTableModule } from '@angular/cdk/table';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatSelectModule,
  MatSidenavModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule
} from '@angular/material';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE
} from '@angular/material/core';
import { TextMaskModule } from 'angular2-text-mask';
import { ContractService } from 'app/contracts/contract.service';
import * as _moment from 'moment';
import { NgHttpLoaderModule } from 'ng-http-loader/ng-http-loader.module';
import { NgxBarcodeModule } from 'ngx-barcode';
import { TreeModule } from 'primeng/components/tree/tree';
import { MaterialsService } from '../materials/services/materials.service';
import { ProtectionDocsService } from '../protection-docs/protection-docs.service';
import { AutoAllocationService } from '../requests/auto-allocation.service';
import { RequestService } from '../requests/request.service';
import { AuthenticationModule } from './authentication/authentication.module';
import { BarcodeDialogComponent } from './components/barcode-dialog/barcode-dialog.component';
import { BulletinFormComponent } from './components/bulletin-form/bulletin-form.component';
import { CommonAutocompleteComponent } from './components/common-autocomplete/common-autocomplete.component';
import { CommonInputStringComponent } from './components/common-input-string/common-input-string.component';
import { DatepickerComponent } from './components/datepicker/datepicker.component';
import { DeleteDialogComponent } from './components/delete-dialog/delete-dialog.component';
import { SearchModeToggleComponent } from './components/search-mode-toggle/search-mode-toggle.component';
import { TableComponent } from './components/table/table.component';
import { TreeFormDialogComponent } from './components/tree-form-dialog/tree-form-dialog.component';
import { UndoDialogComponent } from './components/undo-dialog/undo-dialog.component';
import { WorkflowDialogComponent } from './components/workflow-dialog/workflow-dialog.component';
import { InputFileComponent } from './input-file.component';
import { KeysPipe } from './pipes/key.pipe';
import { RoundPipe } from './pipes/round.pipe';
import { AccessService } from './services/access.service';
import { BulletinService } from './services/bulletin.service';
import { CustomerService } from './services/customer.service';
import { DictionaryService } from './services/dictionary.service';
import { DocumentsService } from './services/documents.service';
import { GuidService } from './services/guid.service';
import { NcaLayerApiService } from './services/nca-layer-api.service';
import { RouteStageService } from './services/route-stage.service';
import { TreeNodeService } from './services/tree-node.service';
import { WorkflowService } from './services/workflow.service';
import { AddressSearchComponent } from './components/address-search/address-search.component';
import { CommentComponent } from './components/comment/comment.component';
import { SearchPaymentsComponent } from './components/search-payments/search-payments.component';
import { IncomingAnswerComponent } from './components/incoming-answer/incoming-answer.component';
import { CurrentNumberAndStageLabelComponent } from './components/current-number-and-stage-label/current-number-and-stage-label.component';
import { SystemSettingsService } from './services/system-settings.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxSelectModule } from 'ngx-select-ex';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { RightSidenavComponent } from './components/right-sidenav/right-sidenav.component';
import { ImageViewerModule } from 'ng2-image-viewer';
import { ColumnConfigModule } from '../modules/column-config';
import { OkDialogComponent } from './components/ok-dialog/ok-dialog.component';
import { YesNoCancelDialogComponent } from './components/yes-no-cancel-dialog/yes-no-cancel-dialog.component';
import { ItemDetailsComponent } from './components/item-details/item-details.component';
import { SystemService } from './services/system.service';
import { StateService } from './services/state.service';

export const MY_FORMATS = {
  parse: {
    dateInput: 'DD.MM.YYYY'
  },
  display: {
    dateInput: 'DD.MM.YYYY',
    monthYearLabel: 'MM YYYY',
    dateA11yLabel: 'DD.MM.YYYY',
    monthYearA11yLabel: 'MM YYYY'
  }
};

_moment.locale('ru-RU');
export const moment = _moment;

const modules = [
  ColumnConfigModule,
  ImageViewerModule,
  CommonModule,
  FormsModule,
  ReactiveFormsModule,
  FlexLayoutModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatMenuModule,
  MatIconModule,
  MatButtonModule,
  MatSnackBarModule,
  MatToolbarModule,
  MatTabsModule,
  MatProgressBarModule,
  MatInputModule,
  MatTableModule,
  MatPaginatorModule,
  MatSortModule,
  CdkTableModule,
  MatGridListModule,
  MatTooltipModule,
  MatDialogModule,
  MatSelectModule,
  MatCheckboxModule,
  MatSidenavModule,
  MatCardModule,
  MatAutocompleteModule,
  MatListModule,
  NgxBarcodeModule,
  TextMaskModule,
  MatExpansionModule,
  TreeModule,
  MatSlideToggleModule,
  MatChipsModule,
  MatButtonToggleModule
];

@NgModule({
  imports: [
    modules,
    AuthenticationModule,
    HttpClientModule,
    NgHttpLoaderModule,
    NgxPaginationModule,
    NgxSelectModule
  ],
  exports: [
    modules,
    HttpClientModule,
    NgHttpLoaderModule,
    RoundPipe,
    KeysPipe,
    InputFileComponent,
    UndoDialogComponent,
    DeleteDialogComponent,
    TreeFormDialogComponent,
    TreeFormDialogComponent,
    CommonAutocompleteComponent,
    TableComponent,
    CommonInputStringComponent,
    DatepickerComponent,
    WorkflowDialogComponent,
    BulletinFormComponent,
    SearchModeToggleComponent,
    BarcodeDialogComponent,
    AddressSearchComponent,
    CommentComponent,
    SearchPaymentsComponent,
    IncomingAnswerComponent,
    CurrentNumberAndStageLabelComponent,
    SidenavComponent,
    ItemDetailsComponent,
    NgxSelectModule
  ],
  declarations: [
    RoundPipe,
    KeysPipe,
    InputFileComponent,
    UndoDialogComponent,
    DeleteDialogComponent,
    TreeFormDialogComponent,
    TableComponent,
    CommonAutocompleteComponent,
    CommonInputStringComponent,
    DatepickerComponent,
    WorkflowDialogComponent,
    BulletinFormComponent,
    SearchModeToggleComponent,
    BarcodeDialogComponent,
    AddressSearchComponent,
    CommentComponent,
    SearchPaymentsComponent,
    IncomingAnswerComponent,
    CurrentNumberAndStageLabelComponent,
    SidenavComponent,
    RightSidenavComponent,
    OkDialogComponent,
    YesNoCancelDialogComponent,
    ItemDetailsComponent
  ],
  bootstrap: [
    BarcodeDialogComponent,
    IncomingAnswerComponent,
    SearchPaymentsComponent
  ],
  providers: [
    DictionaryService,
    RequestService,
    ProtectionDocsService,
    CustomerService,
    RouteStageService,
    MaterialsService,
    DocumentsService,
    ContractService,
    NcaLayerApiService,
    GuidService,
    TreeNodeService,
    WorkflowService,
    AutoAllocationService,
    BulletinService,
    AccessService,
    SystemSettingsService,
    { provide: MAT_DATE_LOCALE, useValue: 'ru-RU' },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE]
    },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    SystemService,
    StateService
  ],
  entryComponents: [BarcodeDialogComponent]
})
export class SharedModule {}
