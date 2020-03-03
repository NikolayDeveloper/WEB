import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DutyCertData, CertificatesType, AlgorithmType } from '../../shared/models/certificate-model';
import { NcaLayerApiService } from '../../shared/services/nca-layer-api.service';
import { SnackBarHelper } from '../../core/snack-bar-helper.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AuthenticationService } from '../../shared/authentication/authentication.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-dialog-for-password',
  template:
    `<div mat-dialog-content class="form-full-width">
      <mat-form-field>
        <input  matInput
                tabindex="1"
                [(ngModel)]="password"
                placeholder="Password"
                type="password"
                i18n-placeholder="@@loginPassword"
                [ngClass]="{'wrong-password':isWrongPassword}">
      </mat-form-field>
    </div>
    <div mat-dialog-actions align="end">
      <button mat-button (click)="onNoClick()" color="warn" tabindex="-1">Сancel</button>
      <button mat-button tabindex="2" (click)="onOkClick()" color="primary">Ok</button>
    </div>`,
  styles: [
    `.wrong-password{
      border: 2px solid red;
    }`]
})
export class DialogForPasswordComponent implements OnInit, OnDestroy {
  public password: string;
  public isWrongPassword = false;
  dutyCertData: DutyCertData;
  private onDestroy = new Subject();
  constructor(
    public dialogRef: MatDialogRef<DialogForPasswordComponent>,
    private ncaLayerApiService: NcaLayerApiService,
    private snackBarHelper: SnackBarHelper,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.dutyCertData = data.dutyCertData;
    this.password = data.certPassword;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick() {
    this.dutyCertData.password = this.password;
    this.getCertInfo();
  }

  ngOnInit() {
    if (this.password) {
      this.dutyCertData.password = this.password;
      this.getCertInfo();
    }
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  private getCertInfo() {
    this.ncaLayerApiService.getKeys(this.dutyCertData, this.data.certificatesType);
    this.ncaLayerApiService.NCALayer.subscribe(result => {
      if (result.errorCode === 'WRONG_PASSWORD') {
        this.isWrongPassword = true;
        this.password = '';
        return;
      }
      if (result.errorCode === 'LOAD_KEYSTORE_ERROR') {
        this.dialogRef.close({
          LOAD_KEYSTORE_ERROR: true,
          certData: this.dutyCertData,
          username: this.dutyCertData.owner,
          password: this.password
        });
        return;
      }
      if (result.errorCode !== 'NONE') {
        this.snackBarHelper.error('Неизвестная ошибка: ' + result.errorCode);
        return;
      }
      this.dutyCertData.setCertKeyInfo(result.result);
      if (this.dutyCertData.keyAlias == null) {
        this.snackBarHelper.error('Не удалось извлечь информацию');
        return;
      }
      if (!this.dutyCertData.isRightAlgorithm()) {
        this.snackBarHelper.error(`Выбран некорректный тип сертификата. Укажите файл сертификата ${this.dutyCertData.expectedAlgorithm}.`);
        return;
      }
      this.loadSert();
    });
  }

  private loadSert() {
    this.ncaLayerApiService.getNotAfter(this.dutyCertData);

    this.ncaLayerApiService.NCALayer.subscribe(result => {
      this.dutyCertData.validDataTo = result.result.split(' ')[0];
      this.setInfoNotBefore();
    });
  }
  private setInfoNotBefore() {
    this.ncaLayerApiService.getNotBefore(this.dutyCertData);

    this.ncaLayerApiService.NCALayer.subscribe(result => {
      this.dutyCertData.validDataFrom = result.result.split(' ')[0];
      this.setInfoSubjectDN();
    });
  }

  private setInfoSubjectDN() {
    this.ncaLayerApiService.getSubjectDN(this.dutyCertData);
    this.ncaLayerApiService.NCALayer.subscribe(result => {
      this.dutyCertData.setInfoDn(result.result);
      this.setCertInfo();
    });
  }

  private setCertInfo() {
    this.ncaLayerApiService.signXml(this.dutyCertData.storageName, this.dutyCertData.storagePath,
      this.dutyCertData.keyAlias, this.dutyCertData.password, '<xml></xml>');
    this.ncaLayerApiService.NCALayer.subscribe(result => {
      this.dutyCertData.setCertData(result.result);
      this.dialogRef.close({
        LOAD_KEYSTORE_ERROR: false,
        certData: this.dutyCertData,
        username: this.dutyCertData.owner,
        password: this.password
      });
    });
  }
}
