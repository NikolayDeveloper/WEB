import { Operators } from '../../shared/filter/operators';

export class Payment {
    id: number;
    paymentNumber: string;
    amount: number;
    paymentDate: Date;
    payment1CNumber: string;
    remainderAmount: number;
    paymentUseAmountSum: number;
    customerId: number;
    customerXin: string;
    customerNameRu: string;
    returnedAmount: number;
    blockedAmount: number;
}

export const OperatorFor = {
    paymentNumber: Operators.like,
    paymentDateFrom: Operators.greaterThanEqual,
    paymentDateTo: Operators.lessThan,
    customerId: Operators.equal,
    customerXin: Operators.equal,
    customerNameRu: Operators.like,
    amountFrom: Operators.greaterThanEqual,
    amountTo: Operators.lessThan,
};

export class PaymentUse {
    paymentId: number;
    paymentInvoiceId: number;
    amount: number;
    createUserId: number;
    description: string;

    public constructor(init?: Partial<PaymentUse>) {
        Object.assign(this, init);
    }
}

export class PaymentInvoice {
    id: number;
    ownerId: number;
    tariffId: number;
    tariffNameRu: string;
    coefficient: number;
    tariffCount: number;
    tariffCode: string;
    penaltyPercent: number;
    nds: number;
    tariffPrice: number;
    totalAmount: number;
    totalAmountNds: number;
    amountUseSum: number;
    remainder: number;
    statusId: number;
    statusCode: string;
    statusNameRu: string;
    createDate: Date;
    createUser: string;
    creditDate: Date;
    creditUser: string;
    writeOffDate: Date;
    writeOffUser: string;
    deleteDate: Date;
    deleteUser: string;
    deletionReason: string;
    isDeleted: boolean;

    public constructor(init?: Partial<PaymentInvoice>) {
        if (!init || !init['id']) {
            this['id'] = 0;
        }

        Object.assign(this, init);
    }
}
