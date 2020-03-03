import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Config } from '../table/config.model';
import { IncomingAnswerService } from './incoming-answer.service';
import { MaterialTask } from 'app/materials/models/materials.model';

@Component({
  selector: 'app-incoming-answer',
  templateUrl: './incoming-answer.component.html',
  styleUrls: ['./incoming-answer.component.scss']
})
export class IncomingAnswerComponent  implements OnDestroy {
  isSelectedRow = false;
  private onDestroy = new Subject();
  columns: Config[] = [
    new Config({ columnDef: 'incomingNumber', header: 'Номер', class: 'width-100' }),
    new Config({ columnDef: 'typeNameRu', header: 'Тип', class: 'width-250' }),
    new Config({ columnDef: 'barcode', header: 'Штрихкод', class: 'width-75' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата создания', class: 'width-200' }),
    new Config({ columnDef: 'currentStageUser', header: 'Исполнитель', class: 'width-150' }),
  ];
  selectRow: MaterialTask;
  get source() { return this.incomingAnswerService; }

  constructor(private dialog: MatDialog,
    private incomingAnswerService: IncomingAnswerService,
    private dialogRef: MatDialogRef<IncomingAnswerComponent>) {
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmit() {
    this.dialogRef.close(this.selectRow);
  }

  onSelect(record: MaterialTask) {
    this.isSelectedRow = true;
    this.selectRow = record;
  }

  onCancel() {
    this.dialogRef.close();
  }
}
