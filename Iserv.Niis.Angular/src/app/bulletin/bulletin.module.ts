import { NgModule } from '@angular/core';
import { BulletinRoutingModule } from './bulletin-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { CreateBulletinSectionDialogComponent } from './components/create-bulletin-section-dialog/create-bulletin-section-dialog.component';
import { SectionsListComponent } from './components/sections-list/sections-list.component';
import { BulletinSectionsService } from './services/bulletin-sections.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { DateHttpInterceptor } from 'app/payments-journal/helpers/date-http-interceptor';
import { EditBulletinSectionDialogComponent } from './components/edit-bulletin-section-dialog/edit-bulletin-section-dialog.component';
import { ModalService } from 'app/shared/services/modal.service';
import { BulletinSectionsComponent } from './components/bulletin-sections/bulletin-sections.component';
import { BulletinSectionDetailsComponent } from './components/bulletin-section-details/bulletin-section-details.component';
import { CreateBulletinDialogComponent } from './components/create-bulletin-dialog/create-bulletin-dialog.component';
import { BulletinsService } from './services/bulletins.service';

@NgModule({
  imports: [
    SharedModule,
    BulletinRoutingModule
  ],
  declarations: [
    CreateBulletinSectionDialogComponent,
    SectionsListComponent,
    EditBulletinSectionDialogComponent,
    BulletinSectionsComponent,
    BulletinSectionDetailsComponent,
    CreateBulletinDialogComponent
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DateHttpInterceptor,
      multi: true
    },
    BulletinSectionsService,
    BulletinsService,
    ModalService
  ],
  entryComponents: [
    CreateBulletinSectionDialogComponent,
    EditBulletinSectionDialogComponent,
    CreateBulletinDialogComponent
  ]
})
export class BulletinModule { }
