import { AuthenticationService } from '../shared/authentication/authentication.service';
import { UsersService } from 'app/administration/users.service';
import { LoginModel } from '../shared/authentication/authentication.model';
import { Component, OnInit } from '@angular/core';
import { DialogForPasswordComponent } from '../login/dialog/dialog-for-password.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { SnackBarHelper } from '../core/snack-bar-helper.service';
import { DutyCertData, CertificatesType, AlgorithmType } from '../shared/models/certificate-model';
import { GuidService } from '../shared/services/guid.service';
import { NcaLayerApiService } from '../shared/services/nca-layer-api.service';
import { UserDetails } from 'app/administration/components/users/models/user.model';
import { TokenStorageService } from 'app/shared/authentication/token-storage.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isCertificate = false;
  dutyCertData: DutyCertData;
  loginModel: LoginModel;
  isCorrectPassword = true;
  username: string;
  password: string;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private usersService: UsersService,
    private tokenStorageService: TokenStorageService,
    private authenticationService: AuthenticationService,
    private snackBarHelper: SnackBarHelper,
    private guidService: GuidService,
    private ncaLayerApiService: NcaLayerApiService,
    public dialog: MatDialog) {
    this.loginForm = fb.group({
      byLogin: fb.group({
        userName: ['', [Validators.required, Validators.minLength(6)]],
        password: ['', [Validators.required, Validators.minLength(6)]],
      }),
      digitalSignatureInfo: fb.group({
        iin: [{ value: '', disabled: true }],
        bin: [{ value: '', disabled: true }],
        fullName: [{ value: '', disabled: true }],
        validDate: [{ value: '', disabled: true }],
      }),
      byDigitalSignature: fb.group({
        certData: ['', Validators.required],
        plainData: ['', Validators.required],
        signedPlainData: ['', Validators.required],
        userName: [''],
        password: [''],
      })
    });
  }

  ngOnInit() {
    this.loginModel = new LoginModel();
    this.dutyCertData = new DutyCertData(this.guidService.generateGuid(), AlgorithmType.RSA);
    this.loginForm.get('byDigitalSignature').get('plainData').setValue(this.dutyCertData.data);
  }

  onSubmit() {
    this.loginForm.markAsPristine();
    this.resetLoginModel();
    if (this.isCertificate) {
      Object.assign(this.loginModel, this.loginForm.get('byDigitalSignature').value);
      Object.assign(this.loginModel, this.loginForm.get('digitalSignatureInfo').value);
      this.loginModel.isCertificate = this.isCertificate;
    } else {
      Object.assign(this.loginModel, this.loginForm.get('byLogin').value);
      this.loginModel.isCertificate = this.isCertificate;
    }
    this.authenticationService
      .login(this.loginModel)
      .subscribe(
        () => {
          this.usersService.getCurrent()
          .subscribe((data: UserDetails) => {
            this.tokenStorageService.setUserData(data);
          });

          this.router.navigateByUrl('/');
        },
        (error) => this.snackBarHelper.error(error.error));
  }

  public onBrowse(value: any) {
    throw new Error('Not implemented');
  }

  public setIndexFocus(event) {
    if (event.index === 1) {
      this.isCertificate = true;
    } else {
      this.isCertificate = false;
    }
  }
  public isFormValid() {
    if (this.isCertificate) {
      const formCert = this.loginForm.get('byDigitalSignature');
      return formCert.valid;
    } else {
      const formLogin = this.loginForm.get('byLogin');
      return formLogin.valid;
    }
  }
  private resetLoginModel() {
    this.loginModel.certData = '';
    this.loginModel.password = '';
    this.loginModel.plainData = '';
    this.loginModel.signedPlainData = '';
    this.loginModel.userName = '';
  }

  // Открывает окно выбора сертификата
  public browseCertificate() {
    this.ncaLayerApiService.browseKeyStore(this.dutyCertData.storageName, this.dutyCertData.fileExtension, '');

    this.ncaLayerApiService.NCALayer.subscribe(result => {
      if (result.errorCode !== 'NONE') {
        this.snackBarHelper.error('Ошибка');
        return;
      }
      if (!result.result) {
        return;
      }
      this.dutyCertData.storagePath = result.result;
      this.openDialog(true);
    });
  }

  private openDialog(isCorrectPassword: boolean): void {
    const dialogRef = this.dialog.open(DialogForPasswordComponent, {
      width: '300px',
      data: {
        dutyCertData: this.dutyCertData,
        certificatesType: CertificatesType.AUTH
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loginForm.markAsDirty();
        this.dutyCertData = result.certData;
        this.username = result.username;
        this.password = result.password;
        this.signPlainData();
      }
    });
  }

  private signPlainData() {
    this.ncaLayerApiService.signPlainData(this.dutyCertData.storageName, this.dutyCertData.storagePath,
      this.dutyCertData.keyAlias, this.dutyCertData.password, this.dutyCertData.data);
    this.ncaLayerApiService.NCALayer.subscribe(result => {
      this.dutyCertData.signedData = result.result;
      this.setCertData();
    });
  }

  private setCertData() {
    this.loginForm.get('byDigitalSignature').get('certData').setValue(this.dutyCertData.certData);
    this.loginForm.get('byDigitalSignature').get('signedPlainData').setValue(this.dutyCertData.signedData);
  }
}

