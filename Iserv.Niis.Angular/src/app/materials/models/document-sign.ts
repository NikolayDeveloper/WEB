export class DocumentSign {
    id: number;
    workflowId: number;
    userId: number;
    plainData: string;
    signedData: string;
    signerCertificate: string;
    password: string;
    certStoragePath: string;
    signatureDate: Date;
    public constructor(init?: Partial<DocumentSign>) {
        Object.assign(this, init);
    }
}
