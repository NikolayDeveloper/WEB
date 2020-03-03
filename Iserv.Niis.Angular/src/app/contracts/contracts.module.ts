import { UsersService } from '../administration/users.service';
import { NgModule } from '@angular/core';
import { WorkflowBusinessService } from 'app/contracts/services/workflow-business.service';

import { MaterialsModule } from '../materials/materials.module';
import { DeleteDialogComponent } from '../shared/components/delete-dialog/delete-dialog.component';
import { SharedModule } from '../shared/shared.module';
import { SubjectsService } from '../subjects/services/subjects.service';
import { SubjectsModule } from '../subjects/subjects.module';
import { ContractDetailComponent } from './components/contract-detail-components/contract-detail/contract-detail.component';
import {
    ContractSubjectDescriptionComponent,
} from './components/contract-detail-components/contract-subject/contract-subject-description/contract-subject-description.component';
import {
    ContractSubjectComponent,
} from './components/contract-detail-components/contract-subject/contract-subject.component';
import {
    ContractDescriptionComponent,
} from './components/contract-detail-components/contract/contract-description/contract-description.component';
import { ContractComponent } from './components/contract-detail-components/contract/contract.component';
import { OfficeWorkComponent } from './components/contract-detail-components/office-work/office-work.component';
import { WorkflowDialogComponent } from './components/contract-detail-components/workflow-dialog/workflow-dialog.component';
import { ContractsComponent } from './containers/contracts/contracts.component';
import { ContractService } from './contract.service';
import { ContractsRoutingModule } from './contracts-routing.module';
import { PaymentsModule } from '../payments/payments.module';
import { ContractListComponent } from './components/contract-detail-components/contract-list/contract-list.component';
import { ColumnConfigService } from 'app/modules/column-config';

@NgModule({
  imports: [
    SharedModule,
    ContractsRoutingModule,
    MaterialsModule,
    SubjectsModule,
    PaymentsModule,
  ],
  declarations: [
    ContractDetailComponent,
    ContractsComponent,
    ContractComponent,
    ContractSubjectComponent,
    OfficeWorkComponent,
    ContractDescriptionComponent,
    WorkflowDialogComponent,
    ContractSubjectDescriptionComponent,
    ContractListComponent
  ],
  bootstrap: [DeleteDialogComponent],
  providers: [
    ContractService,
    SubjectsService,
    WorkflowBusinessService,
    UsersService,
    ColumnConfigService
  ],
  exports: [
    ContractListComponent
  ],
  entryComponents: [
    WorkflowDialogComponent
  ]
})
export class ContractsModule { }
