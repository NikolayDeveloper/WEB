import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { Config } from 'app/shared/components/table/config.model';
import { MaterialTask } from 'app/materials/models/materials.model';
import { MatDialog, MatDialogRef } from '@angular/material';
import { MaterialListService } from './material-list.service';

@Component({
  selector: 'app-material-list',
  templateUrl: './material-list.component.html',
  styleUrls: ['./material-list.component.scss']
})
export class MaterialListComponent implements OnDestroy {
  isSelectedRow = false;
  private onDestroy = new Subject();
  columns: Config[] = [
    new Config({ columnDef: 'displayNumber', header: 'Номер', class: 'width-100' }),
    new Config({ columnDef: 'typeNameRu', header: 'Тип', class: 'width-250' }),
    new Config({ columnDef: 'barcode', header: 'Штрихкод', class: 'width-75' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата создания', class: 'width-200' }),
    new Config({ columnDef: 'currentStageUser', header: 'Исполнитель', class: 'width-150' }),
  ];
  selectRow: MaterialTask;
  get source() { return this.materialListService; }

  constructor(private dialog: MatDialog,
    private materialListService: MaterialListService,
    private dialogRef: MatDialogRef<MaterialListComponent>) {
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
