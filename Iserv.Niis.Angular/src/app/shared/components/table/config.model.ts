export class Config {
    columnDef: string;
    header: string;
    class: string;
    format: (row: any) => string;
    click: (row: any) => void;
    disable: (row: any) => boolean;
    icon: string;
    isSticky: boolean;
    secondHeader: string;

    constructor(init?: Partial<Config>) {
        Object.assign(this, init);
    }
}
