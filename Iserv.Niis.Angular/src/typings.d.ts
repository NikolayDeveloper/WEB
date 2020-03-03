/* SystemJS module definition */
declare var module: NodeModule;
interface NodeModule {
  id: string;
}

interface Array<T> {
  sortByDate(dateField: (e: T) => Date, type: string): T[];
  allCodes(): string[];
}
