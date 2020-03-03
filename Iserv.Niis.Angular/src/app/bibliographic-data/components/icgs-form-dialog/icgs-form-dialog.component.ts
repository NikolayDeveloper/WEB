import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogConfig, MatDialog } from '@angular/material';

import { ICGSRequestDto } from '../../models/icgs-request-dto';
import { BaseDictionary } from '../../../shared/services/models/base-dictionary';
import { DataInputFormDialogComponent } from '../data-input-form-dialog/data-input-form-dialog.component';
import { ConfirmFormIcgsDialogComponent } from '../confirm-form-icgs-dialog/confirm-form-icgs-dialog.component';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { SelectOption } from 'app/shared/services/models/select-option';

@Component({
  selector: 'app-icgs-form-dialog',
  templateUrl: './icgs-form-dialog.component.html',
  styleUrls: ['./icgs-form-dialog.component.scss']
})
export class IcgsFormDialogComponent implements OnInit {
  lengthClass = 8;
  icgsRequest: ICGSRequestDto = this.data.icgsRequest;
  dicICGS: any = this.data.dicICGS;
  className: string;
  originDicICGSNames: string[] = [];
  viewDicICGSNames: string[] = [];
  descriptionNames: string[] = [];
  searchText: string;
  constructor(
    private dialog: MatDialog,
    public dialogRef: MatDialogRef<IcgsFormDialogComponent>,
    private dictionaryService: DictionaryService,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.initializeStrings();
  }
  onCancel(): void {
    this.dialogRef.close(null);
  }
  onSave() {
    const services: string = this.descriptionNames.join(';').replace(/[;]+/g, '; ');
    this.dialogRef.close(services);
  }
  isContainsService(name: string): boolean {
    if (name === undefined && name === null) {
      return false;
    }
    // console.log(this.originDicICGSNames);
    const parentDicValues = this.dicICGS.descriptionShort.split(';');
    if (parentDicValues.some(c =>  this.prepareStringForCompare(c) === this.prepareStringForCompare(name))) {
      return true;
    }

    for (const icgsName of this.originDicICGSNames) {
      if (icgsName === undefined || icgsName === null) {
        return false;
      }
      if (this.prepareStringForCompare(icgsName) === this.prepareStringForCompare(name)) {
        return true;
      }
    }
    return false;
  }
  getFoundServices() {
    return this.descriptionNames.filter(x => this.isContainsService(x));
  }
  getNotFoundService() {
    return this.descriptionNames.filter(x => !this.isContainsService(x));
  }
  onChangeName(oldName: string, newName: string) {
    const index = this.descriptionNames.indexOf(oldName);
    if (index !== -1) {
      this.descriptionNames[index] = newName;
    }
  }
  openDialogInputData(oldName: string) {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '35vw';
    config.data = {
      text: oldName
    };
    const dialogRef = this.dialog.open(DataInputFormDialogComponent, config);
    dialogRef.afterClosed()
      .subscribe((result: string) => {
        if (result === null || result === undefined) {
          return;
        }
        this.onChangeName(oldName, result);
      });
  }
  openDialogAddNewService() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '35vw';
    const dialogRef = this.dialog.open(DataInputFormDialogComponent, config);
    dialogRef.afterClosed()
      .subscribe((result: string) => {
        if (result === null || result === undefined) {
          return;
        }
        this.descriptionNames.push(result);
      });
  }
  openDialogDeleteService(nameService: string) {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '35vw';
    const dialogRef = this.dialog.open(ConfirmFormIcgsDialogComponent, config);

    dialogRef.afterClosed()
      .subscribe((result: boolean) => {
        if (result) {
          this.deleteService(nameService);
        }
      });
  }
  deleteService(nameService: string) {
    const index = this.descriptionNames.indexOf(nameService);
    if (index !== -1) {
      this.descriptionNames = this.descriptionNames.filter(str => str !== nameService);
    }
  }
  initializeStrings() {
    // ! this.icgsRequest.icgsId был заменен на this.dicICGS.externalId, т. к. при заполнении справочника имеется связка между внешними ID
    this.dictionaryService.getDetailIcgs(this.dicICGS.externalId)
      .subscribe((data: SelectOption[]) => {
        if (data) {
          this.originDicICGSNames = data.map(x => x.nameRu);
          this.viewDicICGSNames = this.originDicICGSNames;
        }
      });
    const dicDescription = this.prepareString(this.dicICGS.description);
    this.className = dicDescription.substring(0, this.lengthClass);
    this.descriptionNames = this.prepareString(this.icgsRequest.description).split(';');
  }
  prepareString(str: string): string {
    return str.replace(/['"]+/g, '').replace('.', '');
  }
  prepareStringForCompare(str: string): string {
    return str.trim().replace(/\s+/g, '').toLowerCase();
  }
  resetSearch(): void {
    this.searchText = '';
    this.viewDicICGSNames = this.originDicICGSNames;
  }
  search(): void {
    if (this.searchText) {
      this.viewDicICGSNames = this.originDicICGSNames
        .filter(x => x.toLocaleLowerCase().indexOf(this.searchText.toLocaleLowerCase()) !== -1);
    }
  }
}
