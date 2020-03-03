import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatDialogModule, MatSlideToggleModule } from '@angular/material';

import { ColumnConfigDialogComponent } from './column-config-dialog/column-config-dialog.component';
import { ColumnConfigService } from './column-config.service';

// В документации указан импорт из primeng/dragdrop, но при сборке на деплой ангуляр ругался на него
import { DragDropModule } from 'primeng/components/dragdrop/dragdrop';

@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    DragDropModule,
  ],
  declarations: [
    ColumnConfigDialogComponent,
  ],
  providers: [
    ColumnConfigService
  ],
  entryComponents: [
    ColumnConfigDialogComponent
  ]
})
export class ColumnConfigModule { }
