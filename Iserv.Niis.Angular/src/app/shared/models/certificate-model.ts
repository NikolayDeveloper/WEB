
export class CertificatesType {
    public static readonly AUTH: string = 'AUTH';
    public static readonly SIGN: string = 'SIGN';
    public static readonly ALL: string = 'ALL';
}
export class AlgorithmType {
    public static readonly RSA: string = 'RSA';
    public static readonly GOST: string = 'ГОСТ';
}
export class DutyCertData {
    public readonly storageName: string = 'PKCS12';
    public readonly fileExtension: string = 'P12';
    public keyAlias: string;
    public storagePath: string;
    public password: string;
    public data: string;
    public signedData: string;
    public algorithm: string;
    public expectedAlgorithm: string;
    public certData: string;
    // Данные для отображения
    public owner: string;
    public validDataTo: string;
    public validDataFrom: string;
    public iin: string;
    public bin: string;
    constructor(data: string, algorithm: string) {
        this.owner = '';
        this.iin = '';
        this.bin = '';
        this.data = data;
        this.expectedAlgorithm = algorithm;
    }
    public setCertKeyInfo(result) {
        const keyInfo = this.getFirstKeyInfo(result);
        this.algorithm = keyInfo.alg;
        this.keyAlias = keyInfo.alias;
    }
    public setInfoDn(dn: string) {
        const objectDn = this.parseDn(dn);
        this.owner = objectDn['CN'] + ' ' + objectDn['G'];
        this.iin = objectDn['SERIALNUMBER'] ? objectDn['SERIALNUMBER'].substring(3) : '';
        this.bin = objectDn['OU'] ? objectDn['OU'].substring(3) : '';
    }
    public setCertData(signedXmlData: string) {
        const startIndex = signedXmlData.indexOf('<ds:X509Certificate>');
        const lastIndex = signedXmlData.lastIndexOf('</ds:X509Certificate>');
        let certData = signedXmlData.substring(startIndex, lastIndex);
        certData = certData.replace('<ds:X509Certificate>', '');
        this.certData = certData;
    }
    public isRightAlgorithm(): boolean {
        return this.algorithm.toUpperCase() === this.expectedAlgorithm.toUpperCase();
    }
    public getValidDate() {
        if (this.validDataTo && this.validDataFrom) {
            return 'c ' + this.validDataFrom + ' по ' + this.validDataTo;
        }
        return '';
    }
    private parseDn(dn) {
        const parts = dn.split(',');
        const ret = {};
        let lastKey = null;
        for (let i = 0; i < parts.length; i++) {
            const kv = parts[i].split('=');
            if (kv.length === 1) {
                if (lastKey) {
                    ret[lastKey] += kv[0];
                }
                continue;
            }
            const key = kv[0].trim();
            const value = kv[1].trim();
            ret[key] = value;
            lastKey = key;
        }
        return (ret);
    }

    private getFirstKeyInfo(result) {
        const slots = result.split('\n');
        for (let i = 0; i < slots.length; i++) {
            if (!slots[i]) {
                continue;
            }
            const slotParts = slots[i].split('|');

            const keyInfo = {
                alg: slotParts[0],
                title: slotParts[1],
                alias: slotParts[3]
            };
            return (keyInfo);
        }
        return (null);
    }
}
