import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild
} from '@angular/core';
import {
  MatCheckboxChange,
  MatDialog,
  MatIconRegistry,
  MatPaginator,
  MatSort,
  MatButtonToggleChange,
  MatSlideToggleChange,
  MatOptionSelectionChange
} from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { saveAs } from 'file-saver';
import 'rxjs/add/operator/takeUntil';
import { Subject } from 'rxjs/Subject';
import { ConfigService } from '../../../core';
import { Request } from '../../../requests/models/request';
import { RequestService } from '../../../requests/request.service';
import { BaseDataSource } from '../../../shared/base-data-source';
import { Operators } from '../../../shared/filter/operators';
import { JournalService } from '../../journal.service';
import { SelectUserDialogComponent } from '../select-user-dialog/select-user-dialog.component';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import {
  FormGroup,
  FormBuilder
} from '../../../../../node_modules/@angular/forms';
import { WorkflowService } from 'app/shared/services/workflow.service';

@Component({
  selector: 'app-journal-tasks',
  templateUrl: './journal-tasks.component.html',
  styleUrls: ['./journal-tasks.component.scss']
})
export class JournalTasksComponent implements OnInit, OnDestroy {
  id: string;
  displayedColumns = [
    'select',
    'barcode',
    'kind',
    'protectionDocTypeValue',
    'requestNum',
    'dateCreate',
    'currentStageValue',
    'reviewDaysAll',
    'reviewDaysStage',
    'actions'
  ];
  dataSource: BaseDataSource<Request> | null;
  today: Date;
  mode = JournalMode.Standard;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;
  isAllSelected = false;
  selectionMode = SelectionMode.Including;
  selectedItems: Request[] = [];
  queryParams: any[];
  objectKinds: ObjectKind[] = [
    { ownerType: OwnerType.Request, name: 'Заявки' },
    { ownerType: OwnerType.ProtectionDoc, name: 'Охранные документы' },
    { ownerType: OwnerType.Contract, name: 'Договора' }
  ];
  selectedOwnerType: ObjectKind;
  formGroup: FormGroup;

  private onDestroy = new Subject();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private requestService: RequestService,
    private configService: ConfigService,
    private journalService: JournalService,
    private dialog: MatDialog,
    private workflowService: WorkflowService,
    private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer,
    private fb: FormBuilder
  ) {
    iconRegistry.addSvgIcon(
      'request',
      sanitizer.bypassSecurityTrustResourceUrl('./assets/request.svg')
    );
    iconRegistry.addSvgIcon(
      'protection-doc',
      sanitizer.bypassSecurityTrustResourceUrl('./assets/protectionDoc.svg')
    );
    iconRegistry.addSvgIcon(
      'contract',
      sanitizer.bypassSecurityTrustResourceUrl('./assets/contract.svg')
    );
    this.formGroup = fb.group({
      ownerType: ['']
    });
  }

  ngOnInit() {
    this.route.params.takeUntil(this.onDestroy).subscribe(params => {
      this.id = params['id'];
      this.paginator.pageIndex = 0;
      this.paginator.pageSize = this.configService.pageSize;
      this.paginator.pageSizeOptions = this.configService.pageSizeOptions;
      this.selectedOwnerType = this.objectKinds.find(
        o => o.ownerType === OwnerType.Request
      );
      this.formGroup.get('ownerType').patchValue(OwnerType.Request);
      let source;
      this.queryParams = [
        { key: 'isComplete' + Operators.equal, value: this.id !== 'active' },
        { key: 'isActiveProtectionDocument' + Operators.equal, value: false },
        {
          key: 'ownerType' + Operators.equal,
          value: this.selectedOwnerType.ownerType
        }
      ];
      source = new BaseDataSource<Request>(
        this.requestService,
        this.configService,
        this.paginator,
        this.sort,
        this.filter,
        false,
        this.queryParams
      );
      this.dataSource = source;
    });
  }

  onSelect(record: Request) {
    this.router.navigate([record.taskType + 's', record.id]);
  }

  onGenerateIpcNumberClick() {
    this.queryParams = [
      { key: 'isComplete' + Operators.equal, value: this.id !== 'active' },
      { key: 'canGenerateGosNumber' + Operators.equal, value: true },
      { key: 'IsIndustrial' + Operators.equal, value: true },
      {
        key: 'ownerType' + Operators.equal,
        value: this.selectedOwnerType.ownerType
      },
      { key: '_sort', value: 'ipcCodes' },
      { key: '_order', value: 'asc' }
    ];
    this.dataSource.reset(this.queryParams);
    this.mode = JournalMode.GenerateGosNumberIpc;
    this.displayedColumns.splice(5, 0, 'ipcCodes');
  }

  onGenerateOtherNumberClick() {
    this.queryParams = [
      { key: 'isComplete' + Operators.equal, value: this.id !== 'active' },
      { key: 'canGenerateGosNumber' + Operators.equal, value: true },
      { key: 'IsIndustrial' + Operators.equal, value: false },
      {
        key: 'ownerType' + Operators.equal,
        value: this.selectedOwnerType.ownerType
      },
      { key: '_sort', value: 'ipcCodes' },
      { key: '_order', value: 'asc' }
    ];
    this.dataSource.reset(this.queryParams);
    this.mode = JournalMode.GenerateGosNumber;
    this.displayedColumns.splice(5, 0, 'ipcCodes');
  }

  onSendRequestsForwardClick() {
    this.queryParams = [
      { key: 'isComplete' + Operators.equal, value: this.id !== 'active' },
      { key: 'isManualStage' + Operators.equal, value: true },
      ,
      { key: 'isActiveProtectionDocument' + Operators.equal, value: false },
      {
        key: 'ownerType' + Operators.equal,
        value: this.selectedOwnerType.ownerType
      }
    ];
    this.dataSource.reset(this.queryParams);
    this.mode = JournalMode.SendToNextStage;
  }

  ngOnDestroy() {
    this.onDestroy = null;
  }

  confirmSelection() {
    switch (this.mode) {
      case JournalMode.GenerateGosNumber:
        this.generateGosNumber(true);
        break;
      case JournalMode.GenerateGosNumberIpc:
        this.generateGosNumber(false);
        break;
      case JournalMode.SendToNextStage:
        this.SendToNextStage();
        break;
      default:
        break;
    }
  }

  resetSelection() {
    this.queryParams = [
      { key: 'isComplete' + Operators.equal, value: this.id !== 'active' },
      { key: 'isActiveProtectionDocument' + Operators.equal, value: false },
      {
        key: 'ownerType' + Operators.equal,
        value: this.selectedOwnerType.ownerType
      }
    ];
    this.dataSource.reset(this.queryParams);
    this.isAllSelected = false;
    this.selectedItems = [];
    this.mode = JournalMode.Standard;
    const ipcCodesColumnIndex = this.displayedColumns.findIndex(
      f => f === 'ipcCodes'
    );
    if (ipcCodesColumnIndex !== -1) {
      this.displayedColumns.splice(ipcCodesColumnIndex, 1);
    }
  }

  onObjectKindSelectionChange(value: OwnerType) {
    if (!!value) {
      const ownerType = this.objectKinds.find(o => o.ownerType === value);
      this.selectedOwnerType = ownerType;
      this.resetSelection();
    }
  }

  private generateGosNumber(hasIpc: boolean) {
    let elementIds = [];
    this.selectedItems.forEach(element => {
      elementIds.push(element.id)
    });
    const dialogRef = this.dialog.open(SelectUserDialogComponent, {
      width: '700px',
      data: {
        areAllSelectedTrademarks: this.selectedItems.every(s =>
          [
            'Заявка на Товарный Знак',
            'Заявка на Наименование Мест Происхождения Товаров',
            'Свидетельство на Товарный Знак',
            'Свидетельство на Наименование Мест Происхождения Товаров'
          ].includes(s.protectionDocTypeValue)
        ),
        nextStageCode: 'OD01.2.2',
        nextStageIsParalel: this.selectedItems.every(s =>
          [
            'Патент на изобретение',
            'Патент на Полезную Модель',
            'Патент на Промышленный Образец',
            'Патент на селекционное достижение'
          ].includes(s.protectionDocTypeValue)
        ),
        areAccelerated: this.requestService.arePDAccelerated(elementIds)          
      }
    });

    dialogRef.afterClosed().subscribe(userId => {
      if (userId) {
        const params = [
          { key: 'hasIpc', value: hasIpc },
          { key: 'isAllSelected', value: this.isAllSelected },
          { key: 'selectionMode', value: this.selectionMode }
        ];
        const bulletinId = !!userId.bulletinId.id
          ? userId.bulletinId.id
          : userId.bulletinId.bulletinId;
        this.journalService
          .sendProtectionDocsToStage(
            !!userId.currentUserId ? userId.currentUserId : 0,
            !!userId.nextUserForPrintId ? userId.nextUserForPrintId : 0,
            !!userId.nextUserForDescriptionsId ? userId.nextUserForDescriptionsId : 0,
            !!userId.nextUserForMaintenanceId ? userId.nextUserForMaintenanceId : 0,
            !!userId.bulletinUserId ? userId.bulletinUserId : 0,
            bulletinId,
            !!userId.supportUserId ? userId.supportUserId : 0,
            this.selectedItems.map(s => s.id),
            params
          )
          .takeUntil(this.onDestroy)
          .subscribe(() => {
            this.resetSelection();
          });
      }
    });
  }

  private SendToNextStage() {
    this.workflowService
      .getNextStagesByOwnerId(this.selectedItems[0].id, OwnerType.Request)
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        const dialogRef = this.dialog.open(SelectUserDialogComponent, {
          width: '700px',
          data: {
            areSelectedRequests: true,
            nextStageCode: data.length > 0 ? data[0].code : ''
          }
        });

        dialogRef.afterClosed().subscribe(userId => {
          if (userId) {
            const params = [
              { key: 'isAllSelected', value: this.isAllSelected },
              { key: 'selectionMode', value: this.selectionMode }
            ];
            this.journalService
              .sendRequestsToStage(
                userId.currentUserId,
                this.selectedItems.map(s => s.id),
                params
              )
              .takeUntil(this.onDestroy)
              .subscribe(() => {
                this.resetSelection();
              });
          }
        });
      });
  }

  onAttachmentClick(row: Request, isDownload) {
    this.journalService
      .getAttachment(row.ownerType, row.id)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        if (isDownload) {
          saveAs(blob, name);
        } else {
          window.open(window.URL.createObjectURL(blob, { oneTimeOnly: true }));
        }
      });
  }

  isChecked(row: Request): boolean {
    return (
      this.isAllSelected ||
      (this.selectionMode === SelectionMode.Including &&
        this.selectedItems.some(
          si => si.id === row.id && si.ownerType === row.ownerType
        )) ||
      (this.selectionMode === SelectionMode.Except &&
        !this.selectedItems.some(
          si => si.id === row.id && si.ownerType === row.ownerType
        ))
    );
  }

  onCheck(row: Request) {
    const isChecked = !this.selectedItems.some(
      s => s.id === row.id && s.ownerType === row.ownerType
    );
    if (this.selectionMode === SelectionMode.Except) {
      this.isAllSelected = false;
    }
    isChecked
      ? this.selectedItems.push(row)
      : this.selectedItems.splice(
          this.selectedItems.findIndex(
            si => si.id === row.id && si.ownerType === row.ownerType
          ),
          1
        );
  }

  isAllChecked(): boolean {
    return (
      this.isAllSelected ||
      this.dataSource.data.every(d => this.selectedItems.includes(d))
    );
  }

  checkAll(isChecked: MatCheckboxChange) {
    this.selectedItems = [];
    isChecked.checked
      ? (this.isAllSelected = true)
      : (this.isAllSelected = false);
    if (!this.isAllSelected) {
      this.selectionMode = SelectionMode.Including;
    } else {
      this.selectionMode = SelectionMode.Except;
    }
  }

  isSomeChecked(): boolean {
    return (
      this.selectedItems.length > 0 &&
      this.selectedItems.length < this.dataSource.data.length
    );
  }

  toggleActivePds(event: MatSlideToggleChange) {
    if (event.checked) {
      this.queryParams = [
        { key: 'isComplete' + Operators.equal, value: this.id !== 'active' },
        { key: 'isActiveProtectionDocument' + Operators.equal, value: true },
        {
          key: 'ownerType' + Operators.equal,
          value: this.selectedOwnerType.ownerType
        }
      ];
      this.dataSource.reset(this.queryParams);
    } else {
      this.resetSelection();
    }
  }
}

export enum JournalMode {
  Standard = 0,
  GenerateGosNumber = 1,
  GenerateGosNumberIpc = 2,
  SendToNextStage = 3
}

export enum SelectionMode {
  Including = 0,
  Except = 1
}

export class ObjectKind {
  name: string;
  ownerType: OwnerType;
}
