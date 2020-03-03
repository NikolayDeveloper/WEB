import 'rxjs/add/operator/takeUntil';

import { Component, OnDestroy, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { saveAs } from 'file-saver';
import { Subject } from 'rxjs/Subject';

import { ConfigService } from '../../../core';
import { IncomingMaterialsService } from '../../../materials/incoming/incoming-materials.service';
import { getDocumentTypeName, getDocumentTypeRoute, MaterialTask } from '../../../materials/models/materials.model';
import { Config } from '../../../shared/components/table/config.model';
import { DocumentsService } from '../../../shared/services/documents.service';



@Component({
  selector: 'app-incoming-material-tasks',
  templateUrl: './incoming-material-tasks.component.html',
  styleUrls: ['./incoming-material-tasks.component.scss']
})
export class IncomingMaterialTasksComponent implements OnInit, OnDestroy, AfterViewInit {
  id: string;
  private sub: any;
  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({
      columnDef: 'documentType', header: 'Тип корреспонденции',
      class: 'width-100',
      format: (row) => this.getTypeName.call(this, row)
    }),
    new Config({ columnDef: 'typeNameRu', header: 'Тип' }),
    new Config({ columnDef: 'barcode', header: 'Штрихкод', class: 'width-100' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата создания', class: 'width-200' }),
    new Config({ columnDef: 'currentStageUser', header: 'Исполнитель', class: 'width-200' }),
    new Config({ columnDef: 'creator', header: 'Пользователь', class: 'width-200' }),
    new Config({
      columnDef: 'preview', header: 'Предпросмотр', class: 'width-25 sticky',
      icon: 'zoom_in',
      click: (row) => this.onAttachmentClick.call(this, row, false),
      disable: (row) => !row.canDownload
    }),
    new Config({
      columnDef: 'download', header: 'Загрузить', class: 'width-25 sticky',
      icon: 'file_download',
      click: (row) => this.onAttachmentClick.call(this, row, true),
      disable: (row) => !row.canDownload
    }),
  ];
  today: Date;

  get source() { return this.taskService; }

  private onDestroy = new Subject();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: IncomingMaterialsService,
    private documentsService: DocumentsService,
    private changeDetector: ChangeDetectorRef) {
  }

  ngOnInit() {
    this.route.params
      .takeUntil(this.onDestroy)
      .subscribe(params => {
        this.id = params['id'];
      });
  }

  ngAfterViewInit(): void {
    // TODO: workaround против этого бага https://github.com/angular/material2/issues/5593
    this.changeDetector.detectChanges();
  }

  onSelect(record: MaterialTask) {
    this.router.navigate([getDocumentTypeRoute(record.documentType), record.id]);
  }

  ngOnDestroy() {
    this.onDestroy = null;
  }

  onAttachmentClick(row: MaterialTask, isDownload) {
    this.documentsService.getDocumentPdf(row.id, row.wasScanned, true)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        if (isDownload) {
          saveAs(blob, (blob as any).name || name);
        } else {
          window.open(window.URL.createObjectURL(blob, { oneTimeOnly: true }));
        }
      });
  }

  getTypeName(row: MaterialTask) {
    return getDocumentTypeName(row.documentType);
  }
}
