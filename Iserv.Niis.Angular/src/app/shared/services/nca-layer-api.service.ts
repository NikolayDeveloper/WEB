import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { SnackBarHelper } from '../../core/snack-bar-helper.service';
import { DutyCertData } from '../models/certificate-model';

@Injectable()
export class NcaLayerApiService {
  private _webSocket: WebSocket = new WebSocket('wss://127.0.0.1:13579/');
  public NCALayer: Observable<any>;


  constructor(private snackBarHelper: SnackBarHelper) {
    this._webSocket.onopen = () => console.log('Connection opened');
    this._webSocket.onerror = () => this.snackBarHelper.error('Включите NCALayer');
    this.NCALayer = new Observable(observer => {
      this._webSocket.onmessage = (result) => {
        if (result.data) {
          observer.next(JSON.parse(result.data));
        }
      };
    });
  }
  public closeWebSocket() {
    this._webSocket.close();
  }

  public browseKeyStore(storageName, fileExtension, currentDirectory) {
    const browseKeyStore = {
      'method': 'browseKeyStore',
      'args': [storageName, fileExtension, currentDirectory]
    };
    if (this._webSocket.readyState !== 1) {
      this.snackBarHelper.error('Включите NCALayer');
    }
    this._webSocket.send(JSON.stringify(browseKeyStore));
  }

  public getKeys(certData: DutyCertData, typeCert: string) {
    const getKeys = {
      'method': 'getKeys',
      'args': [certData.storageName, certData.storagePath, certData.password, typeCert]
    };
    this._webSocket.send(JSON.stringify(getKeys));
  }

  public getNotBefore(certData: DutyCertData) {
    const getNotBefore = {
      'method': 'getNotBefore',
      'args': [certData.storageName, certData.storagePath, certData.keyAlias, certData.password]
    };
    this._webSocket.send(JSON.stringify(getNotBefore));
  }

  public getNotAfter(certData: DutyCertData) {
    const getNotAfter = {
      'method': 'getNotAfter',
      'args': [certData.storageName, certData.storagePath, certData.keyAlias, certData.password]
    };
    this._webSocket.send(JSON.stringify(getNotAfter));
  }

  public getSubjectDN(certData: DutyCertData) {
    const getSubjectDN = {
      'method': 'getSubjectDN',
      'args': [certData.storageName, certData.storagePath, certData.keyAlias, certData.password]
    };
    this._webSocket.send(JSON.stringify(getSubjectDN));
  }

  public getIssuerDN(certData: DutyCertData) {
    const getIssuerDN = {
      'method': 'getIssuerDN',
      'args': [certData.storageName, certData.storagePath, certData.keyAlias, certData.password]
    };
    this._webSocket.send(JSON.stringify(getIssuerDN));
  }
  public signXml(storageName, storagePath, alias, password, xmlToSign) {
    const signXml = {
      'method': 'signXml',
      'args': [storageName, storagePath, alias, password, xmlToSign]
    };
    this._webSocket.send(JSON.stringify(signXml));
  }

  public verifyXml(xmlSignature) {
    const verifyXml = {
      'method': 'verifyXml',
      'args': [xmlSignature]
    };
    this._webSocket.send(JSON.stringify(verifyXml));
  }

  public getRdnByOid(storageName, storagePath, alias, password, oid, oidIndex) {
    const getRdnByOid = {
      'method': 'getRdnByOid',
      'args': [storageName, storagePath, alias, password, oid, oidIndex]
    };
    this._webSocket.send(JSON.stringify(getRdnByOid));
  }

  public signPlainData(storageName, storagePath, alias, password, dataToSign) {
    const signPlainData = {
      'method': 'signPlainData',
      'args': [storageName, storagePath, alias, password, dataToSign]
    };
    this._webSocket.send(JSON.stringify(signPlainData));
  }

  public verifyPlainData(storageName, storagePath, alias, password, dataToVerify, base64EcodedSignature) {
    const verifyPlainData = {
      'method': 'verifyPlainData',
      'args': [storageName, storagePath, alias, password, dataToVerify, base64EcodedSignature]
    };
    this._webSocket.send(JSON.stringify(verifyPlainData));
  }
}
