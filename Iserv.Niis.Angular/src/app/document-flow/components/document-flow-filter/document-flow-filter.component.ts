import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {OwnerType} from '../../../shared/services/models/owner-type.enum';
import {DictionaryService} from '../../../shared/services/dictionary.service';
import {DocumentFlowService} from '../../services/document-flow.service';
import {DictionaryType} from '../../../shared/services/models/dictionary-type.enum';
import {getDocumentTypeName} from '../../../materials/models/materials.model';
import { TableComponent } from '../../../shared/components/table/table.component';
import {FormControl, FormGroup} from '@angular/forms';
import {debounceTime, distinctUntilChanged, map, startWith} from 'rxjs/operators';
import * as _moment from 'moment';
import {Observable} from 'rxjs/Observable';

@Component({
  selector: 'app-document-flow-filter',
  templateUrl: './document-flow-filter.component.html',
  styleUrls: ['./document-flow-filter.component.scss']
})
export class DocumentFlowFilterComponent implements OnInit {
  formGroup: FormGroup;
  matFormGroup: FormGroup;
  protectionDocTypes: any[] = [];
  contactsTypes: any[] = [];
  tableState: number;
  filterStates: Observable<any>;
  stageFilter: Observable<any>;
  documentFilter: Observable<any>;
  filterTypes: Observable<any>;
  statusFilter: Observable<any>;
  requestAutocompliteValues: any;
  matAutocompliteValues: any;
  documentType: any[] = [];
  typeNameRu: any[] = [];
  currentStageCode: any[] = [];
  requestsType: any[] = [];
  statuses: any[] = [];
  @Input() searchData: any;
  @Input() templateForms: any;
  @Output() docs = new EventEmitter<any>();
  @Output() searchForm = new EventEmitter<any>();
  constructor(private dictionaryService: DictionaryService, private documentFlowService: DocumentFlowService) {
    this.tableState = 1;
    this.requestAutocompliteValues = {
      statusId: null,
      protectionDocTypeId: null,
      currentStageCode: null
    };
    this.matAutocompliteValues = {
      typeId: null,
      documentType: null
    };
  }


  ngOnInit() {
    this.matFormGroup = new FormGroup({
      barcode: new FormControl(),
      typeId: new FormControl(),
      category: new FormControl(),
      incomingNumber: new FormControl(),
      outgoingNumber: new FormControl(),
      createDate_from: new FormControl(),
      createDate_to: new FormControl(),
      documentType: new FormControl(),
    });
    this.formGroup = new FormGroup({
      barcode: new FormControl(),
      statusId: new FormControl(),
      protectionDocTypeId: new FormControl(),
      currentStageCode: new FormControl(),
      contactsTypesValue: new FormControl(),
      regNumber: new FormControl(),
      createDate_from: new FormControl(),
      createDate_to: new FormControl(),
      currentStageDate_from: new FormControl(),
      currentStageDate_to: new FormControl(),
      status: new FormControl(),
      incomingNumber: new FormControl()
    });
    this.getSearchFormValues();
    this.formGroup.valueChanges.pipe(
      debounceTime(2000),
      distinctUntilChanged()
    ).subscribe(value => {
      Object.keys(this.requestAutocompliteValues).forEach(key => {
        if (value[key]) {
          value[key] = this.requestAutocompliteValues[key];
        }
      });
      value.createDate_from = this.dataFormat(value.createDate_from);
      value.createDate_to = this.dataFormat(value.createDate_to);
      value.currentStageDate_from = this.dataFormat(value.currentStageDate_from);
      value.currentStageDate_to = this.dataFormat(value.currentStageDate_to);
      this.documentFlowService.searchFields.next(value);
    });
    this.setFilters(1);
    this.getStatusValues(1);
    this.matFormGroup.valueChanges.pipe(
      debounceTime(2000),
      distinctUntilChanged()
      ).subscribe(value => {
      Object.keys(this.matAutocompliteValues).forEach(key => {
        if (value[key]) {
          value[key] = this.matAutocompliteValues[key];
        }
      });
      value.createDate_from = this.dataFormat(value.createDate_from);
      value.createDate_to = this.dataFormat(value.createDate_to);
      this.documentFlowService.searchMatFields.next(value);
    });
  }


  public onSelectComplete(evt: any, item: any, key: any, isMaterialString: any) {
    if (evt.source.selected) {
      if (isMaterialString) {
        this.matAutocompliteValues[key] = item;
        this.documentFlowService.searchMatFields.next({[key]: item});
      } else {
        this.requestAutocompliteValues[key] = item;
        this.documentFlowService.searchFields.next({[key]: item});
      }
    }
  }
  private setFilters(tableState: any) {
    this.filterStates = this.formGroup.get('protectionDocTypeId').valueChanges
      .pipe(
        startWith(''),
        map(state => {
          switch (tableState) {
            case 1:
              return state ? this._filterStates(state, this.requestsType) : this.requestsType.slice();
              break;
            case 2:
              return state ? this._filterStates(state, this.protectionDocTypes) : this.protectionDocTypes.slice();
              break;
            case 3:
              return state ? this._filterStates(state, this.contactsTypes) : this.contactsTypes.slice();
          }
        })
      );
    this.stageFilter = this.formGroup.get('currentStageCode').valueChanges
      .pipe(
        startWith(''),
        map(state => {
          return state ? this._filterStates(state, this.currentStageCode) : this.currentStageCode.slice();
        })
      );
  }
  private _filterStates(value: string, searchable: any) {
    const filterValue = value.toLowerCase();
    return searchable.filter(state => state.name ? state.name.toLowerCase().indexOf(filterValue) === 0 : null);
  }

  /**
   * Форматирует дату в строку, используя текущую локаль.
   * @param data дата.
   */
  dataFormat(data: any) {
    return data ? new Date(data).toLocaleDateString() : null;
  }
  getSearchFormValues() {
    for (let i = 0; i <= 8; i++) {
      if (getDocumentTypeName(i).length > 0 && i !== 6 && i !== 4 && i !== 5) {
        this.documentType.push({
          key: i, name: getDocumentTypeName(i)
        });
      }
    }
    this.documentFilter = this.matFormGroup.get('documentType').valueChanges
      .pipe(
        startWith(''),
        map(state => {
          return state ? this._filterStates(state, this.documentType) : this.documentType.slice();
        })
      );
    this.dictionaryService.getBaseDictionary(DictionaryType.DicProtectionDocType).subscribe(val => {
      val.forEach(r => {
        if (r.id <= 6) {
          this.requestsType.push({key: r.id, name: r.nameRu || r.nameKz || r.nameEn});
        } else {
          this.protectionDocTypes.push({key: r.id, name: r.nameRu || r.nameKz || r.nameEn});
        }
      });
    });
    this.dictionaryService.getBaseDictionary(DictionaryType.DicContractType).subscribe(val => {
      this.contactsTypes = val.map(d => {
        return { key: d.id, name: d.nameRu || d.nameKz || d.nameEn};
      });
    });
    this.dictionaryService.getBaseDictionary(DictionaryType.DicDocumentType).subscribe(val => {
      this.typeNameRu = val.map(d => {
        return { key: d.id, name: d.nameRu || d.nameKz || d.nameEn};
      });
      this.filterTypes = this.matFormGroup.get('typeId').valueChanges
        .pipe(
          startWith(''),
          map(state => {
            return state ? this._filterStates(state, this.typeNameRu) : this.typeNameRu.slice();
          })
        );
    });
    this.dictionaryService.getBaseDictionary(DictionaryType.DicRouteStage).subscribe(val => {
      this.currentStageCode = val.map(d => {
        if (!d.isDeleted) {
          return { key: d.code, name: d.nameRu || d.nameKz || d.nameEn};
        }
      });

     this.currentStageCode.forEach(stage => {
       this.currentStageCode.forEach(each => {
         if (stage.name.toLowerCase() === each.name.toLowerCase() && stage.key !== each.key) {
            stage.key = `${stage.key};${each.key}`;
         }
       });
     });
      this.currentStageCode = this.getUnique(this.currentStageCode, 'name');
    });
  }
  private getStatusValues(ownerType: number) {
    switch (ownerType) {
      case 1:
        this.dictionaryService.getBaseDictionary(DictionaryType.DicRequestStatus).subscribe(val => {
          this.statuses = val.map(d => {
            return { key: d.id, name: d.nameRu || d.nameKz || d.nameEn};
          });
        });
        break;
      case 2:
        this.dictionaryService.getBaseDictionary(DictionaryType.DicProtectionDocStatus).subscribe(val => {
          this.statuses = val.map(d => {
            return { key: d.id, name: d.nameRu || d.nameKz || d.nameEn};
          });
        });
        break;
      case 3:
        this.dictionaryService.getBaseDictionary(DictionaryType.DicContractStatus).subscribe(val => {
          this.statuses = val.map(d => {
            return { key: d.id, name: d.nameRu || d.nameKz || d.nameEn};
          });
        });
        break;
    }

    this.statuses.forEach(stage => {
      this.statuses.forEach(each => {
        if (stage && stage.name && each && each.name
          && stage.name.toLowerCase() === each.name.toLowerCase()
          && stage.key !== each.key) {
          stage.key = `${stage.key};${each.key}`;
        }
      });
    });
    this.statuses = this.getUnique(this.statuses, 'name');
    this.statusFilter = this.formGroup.get('statusId').valueChanges
      .pipe(
        startWith(''),
        map(state => {
          return state ? this._filterStates(state, this.statuses) : this.statuses.slice();
        })
      );
  }
  getUnique(arr: any, comp: any) {
    const unique = arr
      .map(e => e[comp])
      .map((e, i, final) => final.indexOf(e) === i && i)
      .filter(e => arr[e]).map(e => arr[e]);
    return unique;
  }

}

export class FilterCategory {
  text: string;
  id: FilterCategoryEnum;
}

export enum FilterCategoryEnum {
  RequestsAndMaterials,
  Requests,
  Materials
}

export class IndustrialPropertyObjectType {
  name: string;
  objectType: OwnerType;
}
