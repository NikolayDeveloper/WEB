    import { Observable } from 'rxjs/Observable';
    import { BehaviorSubject, Subject } from 'rxjs/Rx';
    import { MatSort, MatPaginator } from '@angular/material';
    import { DataSource } from '@angular/cdk/collections';

    export class ImageDataSource<T> extends DataSource<any> {
        data: T[] = [];
        public resultLength;
        private dataChange = new Subject();

        push(data: T[]) {
            this.data = data;
            this.resultLength =  this.data.length;
            this.dataChange.next(null);
        }

        constructor(private mdSort?: MatSort,
            private mdPaginator?: MatPaginator) {
            super();
        }

        connect(): Observable<T[]> {
            const displayDataChanges = [
                this.mdPaginator.page,
                this.mdSort.sortChange,
                this.dataChange
            ];

            return Observable.merge(...displayDataChanges)
            .startWith(null)
            .map(() => {
                const data = this.getSortedData();
                const startIndex = this.mdPaginator.pageIndex * this.mdPaginator.pageSize;
                return data.slice(startIndex, startIndex + this.mdPaginator.pageSize);
            });
        }

        disconnect() { }

        getSortedData(): T[] {
            if (!this.mdSort || !this.mdSort.active || this.mdSort.direction === '') { return this.data; }

            return this.data.sort((a, b) => {
                let propertyA: number | string = '';
                let propertyB: number | string = '';
                [propertyA, propertyB] = [a[this.mdSort.active], b[this.mdSort.active]];

                const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
                const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

                return (valueA < valueB ? -1 : 1) * (this.mdSort.direction === 'asc' ? 1 : -1);
            });
        }
    }