export { }

Array.prototype.sortByDate = function (propertyCallback: (e: any) => Date, type: string = 'asc'): any[] {
    if (this.length === 0) {
        return this;
    }

    const propertyName = Object.keys(this[0]).find(key => this[0][key] === propertyCallback(this[0]));

    switch (type) {
        case 'desc':
            return this.sort((e1, e2) => new Date(e2[propertyName]).getTime() - new Date(e1[propertyName]).getTime());
        case 'asc':
            return this.sort((e1, e2) => new Date(e1[propertyName]).getTime() - new Date(e2[propertyName]).getTime());
        default:
            throw Error(`'${type}' is incompatible sort operator! You could use only 'asc' or 'desc' operators`);
    }
}

Array.prototype.allCodes = function (): string[] {
    if (this.length === 0) {
        return this;
    }

    return this.map(f => f.codes).reduce((f1, f2) => f1.concat(f2));
}
