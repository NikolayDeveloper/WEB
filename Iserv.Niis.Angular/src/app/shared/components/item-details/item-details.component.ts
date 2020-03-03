import {Component,
  OnInit,
  OnChanges,
  SimpleChanges,
  OnDestroy,
  AfterViewInit,
  Input,
  Output,
  EventEmitter,
  ChangeDetectorRef} from '@angular/core';
import {DictionaryType} from '../../services/models/dictionary-type.enum';
import { RequestService } from '../../../requests/request.service';
import {Subject} from 'rxjs/Subject';
import {DomSanitizer} from '@angular/platform-browser';
import {DocumentsService} from '../../services/documents.service';
import {DictionaryService} from '../../services/dictionary.service';
import {SubjectsService} from '../../../subjects/services/subjects.service';
import {Router} from '@angular/router';
import {ProtectionDocsService} from '../../../protection-docs/protection-docs.service';
import {Observable} from 'rxjs/Observable';
import {ContractService} from '../../../contracts/contract.service';
import * as _moment from 'moment';
import { getDocumentTypeRoute } from 'app/materials/models/materials.model';

_moment.locale('ru-RU');
const moment = _moment;

@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.scss'],
  providers: [SubjectsService]
})
export class ItemDetailsComponent implements OnInit, OnChanges, OnDestroy, AfterViewInit {
  public docId;
  public documentType;
  public sidebarData;
  public pdfBlob;
  public docPrev;
  private onDestroy = new Subject();
  @Input() itemDetailsState: any;
  @Input() clearState: any;
  @Output() tableState = new EventEmitter<any>();

  constructor(private requestService: RequestService,
              private sanitizer: DomSanitizer,
              private documentsService: DocumentsService,
              private dictionaryService: DictionaryService,
              private subjectsService: SubjectsService,
              private router: Router,
              private protectionDocsService: ProtectionDocsService,
              private contractService: ContractService,
              private changeDetector: ChangeDetectorRef,
  ) {}
  ngOnInit() {
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.sidebarData = void 0;
    this.docPrev = void 0;
    this.tableState.emit(changes.clearState && changes.clearState.currentValue);
    if (changes.itemDetailsState) {
      const { currentValue } = changes.itemDetailsState;
      if (currentValue) {
        this.getDataForSidebar(currentValue.row, currentValue);
      }
    }
    if (changes.clearState) {
      this.tableState.emit(changes.clearState.currentValue);
    }
  }
  ngOnDestroy(): void {
    this.onDestroy.next(null);
  }
  getSubject(id): Observable<any> {
    return this.subjectsService.get(id, 1);
  }
  getDataFromSelect(code, object) {
    if (Array.isArray(object)) {
      const foundObj = object.find(ob => ob.id === code);
      return foundObj ? foundObj.nameRu || foundObj.nameKz || foundObj.nameEn : null;
    } else {
      return object.id === code ?  object.nameRu || object.nameKz || object.nameEn : null;
    }
  }
  getDataForSidebar(row, state) {
    if (row.typeNameRu) {
      this.getDocs(row, state);
    } else if (row.taskType === 'protectiondoc') {
      this.getProtectionDocs(row, state);
    } else if (row.taskType === 'contract') {
      this.getContracts(row, state);
    } else {
      this.getRequestData(row, state);
    }
  }
  getRequestData(row, state) {
    this.requestService.getRequestById(row.id)
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        this.sidebarData = Object.assign(data, row);
        this.requestService.getImage(row.id, row.ownerType)
          .takeUntil(this.onDestroy)
          .subscribe(image => {
            if (image.base64) {
              Object.assign(this.sidebarData, {image: this.sanitizer.bypassSecurityTrustUrl(`data:image/jpg;base64,${image.base64}`)});
            }
            this.getDetails(row, state, data);
          });
        this.dictionaryService.getSelectOptions(DictionaryType.DicRequestStatus)
          .takeUntil(this.onDestroy)
          .subscribe(statuses => {
            this.sidebarData.statusName = statuses.find(status => status.id === this.sidebarData.statusId);
          });
      });
  }
  getContracts(row, state) {
    this.contractService.getById(row.id)
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        this.sidebarData = Object.assign(data, row);
        this.getDetails(row, state, data);
      });
  }
  getProtectionDocs(row, state) {
    this.protectionDocsService.get(row.id)
      .takeUntil(this.onDestroy)
      .subscribe(docs => {
        this.sidebarData = Object.assign(docs, row);
        this.getDetails(row, state, docs);
      });
  }
  getDocs(row, state) {
    this.docId = row.id;
    this.documentType = row.documentType;
    this.documentsService.getDocumentPdf(row.id, row.wasScanned, true)
      .takeUntil(this.onDestroy)
      .subscribe(docs => {
        this.pdfBlob = docs;
        this.docPrev = this.sanitizer.bypassSecurityTrustResourceUrl(window.URL.createObjectURL(docs));
        this.tableState.emit(state);
      }, error => {
        this.docPrev = true;
        this.tableState.emit(state);
      });
  }
  openNewWindowPdf() {
    window.open(window.URL.createObjectURL(this.pdfBlob, { oneTimeOnly: true }));
  }
  goToCard(id, url) {
    if (this.docId) {
      const route = getDocumentTypeRoute(this.documentType);
      this.router.navigate([route, id]);
    } else {
      this.router.navigate([`${url}s`, id]);
    }
  }
  getDetails(row, state, data) {
    this.sidebarData.dateCreate = moment(data.dateCreate).utc().format('DD-MM-YYYY');
    this.dictionaryService.getSelectOptions(DictionaryType.DicReceiveType)
      .takeUntil(this.onDestroy)
      .subscribe(select => {
        this.sidebarData.reciveName = select.find(selection => this.sidebarData.receiveTypeId === selection.id);
        this.sidebarData.protectionDocTypeValue = row.protectionDocTypeValue;
        this.getSubject(row.id)
          .takeUntil(this.onDestroy)
          .subscribe(responce => {
            responce.some(res => {
              this.sidebarData.ordererName = res.nameRu || res.nameKz || res.nameEn;
              return this.sidebarData.ordererName;
            });
          });
      });
    this.dictionaryService.getBaseDictionary(DictionaryType.DicProtectionDocSubType)
      .takeUntil(this.onDestroy)
      .subscribe(subDoc => {
        const filteredDocs = subDoc.filter(dc => dc.typeId === this.sidebarData.protectionDocTypeId);
        this.sidebarData.subType = this.getDataFromSelect(this.sidebarData.requestTypeId, filteredDocs);
      });
    this.tableState.emit(state.state);
  }
  ngAfterViewInit(): void {
    // TODO: workaround против этого бага https://github.com/angular/material2/issues/5593
    // Это не баг. Но пускай будет
    this.changeDetector.detectChanges();
  }
}
