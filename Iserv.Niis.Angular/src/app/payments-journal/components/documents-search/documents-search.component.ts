import { Component, EventEmitter, Output } from '@angular/core';
import { DocumentsSearchParametersDto } from '../../models/documents-search-parameters.dto';
import { SimpleSelectOption } from '../../../shared/services/models/select-option';
import { DocumentsService } from '../../services/documents.service';
import { DocumentCategory } from '../../models/document-category';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { DictionaryItem } from '../../helpers/dictionary-item';

@Component({
  selector: 'app-documents-search',
  templateUrl: './documents-search.component.html',
  styleUrls: ['./documents-search.component.scss']
})
export class DocumentsSearchComponent {
  @Output()
  public applySearch: EventEmitter<DocumentsSearchParametersDto> = new EventEmitter();

  public readonly docCategories: SimpleSelectOption[];
  public docTypes: SimpleSelectOption[] = [];
  public receiveTypes: SimpleSelectOption[] = [];
  public requestSubTypes: SimpleSelectOption[] = [];
  public requestTypes: SimpleSelectOption[] = [];
  public requestStatuses: SimpleSelectOption[] = [];
  public protectionDocStatuses: SimpleSelectOption[] = [];
  public icgses: SimpleSelectOption[] = [];
  public selectionAchieveTypes: SimpleSelectOption[] = [];

  public searchParams = new DocumentsSearchParametersDto();

  private readonly requestCodes = ['B', 'U', 'S2', 'TM', 'PN', 'SA'];
  private readonly protectionDocCodes = ['B_PD', 'U_PD', 'S2_PD', 'TM_PD', 'PN_PD', 'SA_PD'];
  private readonly protectionDocTypeCode = ['01', '02', '03', '04', '05'];

  constructor(
    private documentsService: DocumentsService,
    private dictionaryService: DictionaryService) {

    this.docCategories = documentsService.getDocCategories();

    this.loadReceiveTypes();
    this.loadRequestSubTypes();
    this.loadRequestTypes();
    this.loadRequestStatuses();
    this.loadProtectionDocStatuses();
    this.loadIcgses();
    this.loadSelectionAchieveTypes();
  }

  public onDocCategoryChanged(value: DocumentCategory): void {
    switch (value) {
      case DocumentCategory.Request:
        this.loadRequests();
        break;

      case DocumentCategory.ProtectionDoc:
        this.loadProtectionDocs();
        break;

      case DocumentCategory.Contract:
        this.loadContractTypes();
        break;

      default:
        this.docTypes = [];
        break;
    }

    this.searchParams.docTypeId = null;
  }

  private loadRequests(): void {
    this.dictionaryService.getBaseDictionaryByCodes(DictionaryType.DicProtectionDocType, this.requestCodes)
      .subscribe((data) => {
        const docTypes = data
          .filter(item => item.code !== 'DK')
          .map(item => {
            return { id: item.id, nameRu: item.nameRu };
          });

        this.initDocTypes(docTypes);
      });
  }

  private loadProtectionDocs(): void {
    this.dictionaryService.getBaseDictionaryByCodes(DictionaryType.DicProtectionDocType, this.protectionDocCodes)
      .subscribe((data) => {
        const docTypes = data
          .filter(item => item.code !== 'DK')
          .map(item => {
            return { id: item.id, nameRu: item.nameRu };
          });

        this.initDocTypes(docTypes);
      });
  }

  private loadContractTypes(): void {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicContractType)
      .subscribe((data) => {
        const docTypes = data
          .filter(item => item.code !== 'DK')
          .map(item => {
            return { id: item.id, nameRu: item.nameRu };
          });

        this.initDocTypes(docTypes);
      });
  }

  private initDocTypes(docTypes: SimpleSelectOption[]): void {
    this.docTypes = [];
    this.docTypes.push({ id: null, nameRu: null });
    this.docTypes.push(...docTypes);
  }

  private loadReceiveTypes(): void {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicReceiveType)
      .subscribe(result => {
        this.receiveTypes = [];
        this.receiveTypes.push({ id: null, nameRu: null });
        this.receiveTypes.push(...result);
      });
  }

  private loadRequestSubTypes(): void {
    this.dictionaryService.getBaseDictionaryByCodes(DictionaryType.DicProtectionDocSubType, this.protectionDocTypeCode)
      .subscribe(result => {
        this.requestSubTypes = [];
        this.requestSubTypes.push({ id: null, nameRu: null });
        this.requestSubTypes.push(...result);
      });
  }

  private loadRequestTypes(): void {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicConventionType)
      .subscribe(result => {
        this.requestTypes = [];
        this.requestTypes.push({ id: null, nameRu: null });
        this.requestTypes.push(...result);
      });
  }

  private loadRequestStatuses(): void {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicRequestStatus)
      .subscribe(result => {
        this.requestStatuses = [];
        this.requestStatuses.push({ id: null, nameRu: null });
        this.requestStatuses.push(...result);
      });
  }

  private loadProtectionDocStatuses(): void {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicProtectionDocStatus)
      .subscribe(result => {
        this.protectionDocStatuses = [];
        this.protectionDocStatuses.push({ id: null, nameRu: null });
        this.protectionDocStatuses.push(...result);
      });
  }

  private loadIcgses(): void {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicICGS)
      .subscribe(result => {
        this.icgses = [];
        this.icgses.push({ id: null, nameRu: null });
        this.icgses.push(...result);
      });
  }

  private loadSelectionAchieveTypes(): void {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicSelectionAchieveType)
      .subscribe(result => {
        this.selectionAchieveTypes = [];
        this.selectionAchieveTypes.push({ id: null, nameRu: null });
        this.selectionAchieveTypes.push(...result);
      });
  }

  public onDocTypeChanged(value: number): void {
  }

  public onApplySearch(): void {
    this.emitApplySearch();
  }

  public onResetSearch(): void {
    this.searchParams = new DocumentsSearchParametersDto();
    this.emitApplySearch();
  }

  private emitApplySearch(): void {
    const searchParams = new DocumentsSearchParametersDto();
    Object.assign(searchParams, this.searchParams);
    this.applySearch.emit(searchParams);
  }
}
