import { NgModule } from '@angular/core';

import { DeleteDialogComponent } from '../shared/components/delete-dialog/delete-dialog.component';
import { TreeFormDialogComponent } from '../shared/components/tree-form-dialog/tree-form-dialog.component';
import { UndoDialogComponent } from '../shared/components/undo-dialog/undo-dialog.component';
import { SharedModule } from '../shared/shared.module';
import { SubjectsModule } from '../subjects/subjects.module';
import { BibliographicDataService } from './bibliographic-data.service';
import { BibliographicDataComponent } from './components/bibliographic-data.component';
import { ConfirmFormDialogComponent } from './components/confirm-form-dialog/confirm-form-dialog.component';
import { ConfirmFormIcgsDialogComponent } from './components/confirm-form-icgs-dialog/confirm-form-icgs-dialog.component';
import { ConfirmFormRequestConventionDialogComponent } from './components/confirm-form-request-convention-dialog/confirm-form-request-convention-dialog.component';
import { CreateIcgsRequestDialogComponent } from './components/create-icgs-request-dialog/create-icgs-request-dialog.component';
import { DataInputFormDialogComponent } from './components/data-input-form-dialog/data-input-form-dialog.component';
import { DescriptionComponent } from './components/description/description.component';
import { IcgsFormDialogComponent } from './components/icgs-form-dialog/icgs-form-dialog.component';
import { IcgsInfoFormDialogComponent } from './components/icgs-info-form-dialog/icgs-info-form-dialog.component';
import { RequestConventionInfoDialogComponent } from './components/request-convention-info-dialog/request-convention-info-dialog.component';
import { IcgsSplitDialogComponent } from './components/icgs-split-dialog/icgs-split-dialog.component';
import { TreeFormDialogIpcComponent } from './components/tree-form-dialog-ipc/tree-form-dialog-ipc.component';
import { IcisSelectionDialogComponent } from './components/icis-selection-dialog/icis-selection-dialog.component';
import { CommonFieldsComponent } from './components/common-fields/common-fields.component';
import { TrademarkFieldsComponent } from './components/trademark-fields/trademark-fields.component';
import { ProductFieldsComponent } from './components/product-fields/product-fields.component';
import { SelectionFieldsComponent } from './components/selection-fields/selection-fields.component';
import { IcgsFieldsComponent } from './components/icgs-fields/icgs-fields.component';
import { ColorFieldsComponent } from './components/color-fields/color-fields.component';
import { IcfemFieldsComponent } from './components/icfem-fields/icfem-fields.component';
import { IpcFieldsComponent } from './components/ipc-fields/ipc-fields.component';
import { IcisFieldsComponent } from './components/icis-fields/icis-fields.component';
// import { PriorityFieldsComponent } from './components/priority-fields/priority-fields.component';
import { ReferatFieldsComponent } from './components/referat-fields/referat-fields.component';
import { ImageFieldsComponent } from './components/image-fields/image-fields.component';
import { FieldChangeComponent } from './components/changes/field-change/field-change.component';
import { ChangeTypeChooseComponent } from './components/changes/change-type-choose/change-type-choose.component';
import { MakeChangesComponent } from './components/changes/make-changes/make-changes.component';
import { ConfirmChangesComponent } from './components/changes/confirm-changes/confirm-changes.component';
import { SubjectChangeContainerComponent } from './components/changes/subject-change-container/subject-change-container.component';
import { SubjectChangeComponent } from './components/changes/subject-change/subject-change.component';
import { ChangeDialogComponent } from './components/changes/change-dialog/change-dialog.component';
import { MediaFileFieldsComponent } from './components/media-file-fields/media-file-fields.component';
import { CtmParticipantsFieldDialogComponent } from './components/ctm-participants-field-dialog/ctm-participants-field-dialog.component';
import { PriorityComponent } from './components/priority/priority.component';
import { PriorityContainerComponent } from './components/priority-container/priority-container.component';
import { NavigateOnSelectService } from 'app/expert-search/services/navigate-on-select.service';
import { SystemService } from 'app/shared/services/system.service';

@NgModule({
  imports: [SharedModule, SubjectsModule],
  exports: [
    BibliographicDataComponent,
    DescriptionComponent,
    CommonFieldsComponent
  ],
  declarations: [
    BibliographicDataComponent,
    DescriptionComponent,
    IcgsFormDialogComponent,
    IcgsInfoFormDialogComponent,
    DataInputFormDialogComponent,
    ConfirmFormDialogComponent,
    CreateIcgsRequestDialogComponent,
    ConfirmFormIcgsDialogComponent,
    RequestConventionInfoDialogComponent,
    ConfirmFormRequestConventionDialogComponent,
    IcgsSplitDialogComponent,
    TreeFormDialogIpcComponent,
    IcisSelectionDialogComponent,
    CommonFieldsComponent,
    TrademarkFieldsComponent,
    ProductFieldsComponent,
    SelectionFieldsComponent,
    IcgsFieldsComponent,
    ColorFieldsComponent,
    IcfemFieldsComponent,
    IpcFieldsComponent,
    IcisFieldsComponent,
    // PriorityFieldsComponent,
    ReferatFieldsComponent,
    ImageFieldsComponent,
    FieldChangeComponent,
    ChangeTypeChooseComponent,
    MakeChangesComponent,
    ConfirmChangesComponent,
    SubjectChangeComponent,
    SubjectChangeContainerComponent,
    ChangeDialogComponent,
    MediaFileFieldsComponent,
    CtmParticipantsFieldDialogComponent,
    PriorityComponent,
    PriorityContainerComponent
  ],
  bootstrap: [
    UndoDialogComponent,
    DeleteDialogComponent,
    IcgsFormDialogComponent,
    IcgsInfoFormDialogComponent,
    DataInputFormDialogComponent,
    ConfirmFormDialogComponent,
    CreateIcgsRequestDialogComponent,
    ConfirmFormIcgsDialogComponent,
    TreeFormDialogComponent,
    RequestConventionInfoDialogComponent,
    ConfirmFormRequestConventionDialogComponent,
    IcgsSplitDialogComponent,
    TreeFormDialogIpcComponent,
    IcisSelectionDialogComponent,
    ChangeDialogComponent,
    CtmParticipantsFieldDialogComponent
  ],
  providers: [
    BibliographicDataService,
    NavigateOnSelectService,
    SystemService
  ],
  entryComponents: []
})
export class BibliographicDataModule {}
