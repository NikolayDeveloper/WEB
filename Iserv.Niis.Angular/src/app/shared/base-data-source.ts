import { HttpParams } from '@angular/common/http';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/switchMap';

import { DataSource } from '@angular/cdk/table';
import { ElementRef } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject, Subject } from 'rxjs/Rx';

import { BaseServiceWithPagination } from './base-service-with-pagination';
import { Operators } from './filter/operators';
import { PageResponse } from './page-response';

export class BaseDataSource<TEntity> extends DataSource<TEntity> {
    private refreshSubject = new Subject();
    // private remoteCall;
    private minFilterLength = 3;
    /**Дополнитеьные параметры для строки запроса. Устанавливаются через @function setQueryParameters */
    public additionalQueryParams = [];
    private dataChange = new Subject();

    public resultsLength = 0;
    resetCompleted = new Subject<any>();
    data: TEntity[] = [];

    constructor(private service: BaseServiceWithPagination<TEntity>,
        private configService,
        private paginator: any,
        private sort: MatSort = null,
        private filter: ElementRef = null,
        private fetchOnlyFilteredData: boolean = false,
        private queryParams?: any[]) {
        super();
        this.addOrSetQueryParameters(queryParams);
    }

    push(data: TEntity[]) {
        this.data = data;
        this.resultsLength = this.data.length;
        this.dataChange.next(null);
    }

    connect(): Observable<TEntity[]> {
        const filterObservable = this.filter
            ? Observable.fromEvent(this.filter.nativeElement, 'keyup')
                .debounceTime(this.configService.debounceTime)
                .distinctUntilChanged()
                .filter(() => {
                    return this.filter.nativeElement.value.length >= this.minFilterLength
                        || this.filter.nativeElement.value.length === 0;
                })
            : Observable.of(true);

        const displayDataChanges = [
            this.paginator.page,
            this.sort ? this.sort.sortChange : new Subject(),
            this.refreshSubject,
            filterObservable
        ];

        /** Сбрасываем пагинатор на первую страницу при изменении фильтра или сортировки */
        Observable.merge(...[this.sort ? this.sort.sortChange : new Subject(), filterObservable])
            .subscribe(() => this.paginator.pageIndex = 0);

        return Observable.merge(...displayDataChanges)
            .startWith(null)
            .switchMap(() => this.fetchOnlyFilteredData && this.additionalQueryParams.length === 0
                ? Observable.of(new PageResponse<any>(0, new Array(0)))
                : this.service.getPaged(this.buildQueryString())
            )
            .map((data: PageResponse<any>) => {
                this.data = data.items;
                this.resultsLength = data.total;
                this.resetCompleted.next(data);
                return data.items;
            })
            .catch(() => Observable.of(null));
    }

    disconnect() { }

    reset(queryParams?: any[]) {
        this.additionalQueryParams = [];
        this.paginator.pageIndex = 0;
        if (queryParams) {
            this.addOrSetQueryParameters(queryParams);
        } else {
            if (this.filter) {
                this.filter.nativeElement.value = '';
            }
        }
        this.refreshSubject.next();
    }

    update(queryParam: any) {
      this.paginator.pageIndex = 0;
      this.updateQueryParameters(queryParam);
      this.refreshSubject.next();
    }

    private updateQueryParameters(keyValuePair: any) {
      const key = keyValuePair.key;
      const oldValue = this.additionalQueryParams.filter(d => d.key === key);
      if (oldValue.length === 1) {
        this.additionalQueryParams = [...this.additionalQueryParams.filter(d => d.key !== key)];
      }

      this.additionalQueryParams.push({ key: key, value: keyValuePair.value.toString() });
    }

    /**
     * Добавляет или обновляет значения в @var additionalQueryParams
     * @param keyValuePairs массив параметров для добавления
     */
    private addOrSetQueryParameters(keyValuePairs: any[] = []) {
        keyValuePairs.forEach(param => {
            // приводим все значения к строке перед добавлением в запрос
            this.additionalQueryParams.push({ key: this.buildUniqueKey(param.key), value: param.value.toString() });
        });
    }

    private buildUniqueKey(key: string, descriminator?: number): string {
        let keyWithDescriminator = key;
        if (!descriminator) {
            descriminator = 0;
        } else {
            keyWithDescriminator = key + descriminator;
        }

        if (this.additionalQueryParams.findIndex(x => x.key === keyWithDescriminator) === -1) {
            return keyWithDescriminator;
        } else {
            descriminator++;
            return this.buildUniqueKey(key, descriminator);
        }
    }

    /** Строит QueryString из @var additionalQueryParams. Исключает пустые параметры */
    public getQueryParams() {
      const params = [
          { key: '_page', value: 0 },
          { key: '_limit', value: 0 },
          { key: Operators.fullTextSearch, value: this.filter ? this.filter.nativeElement.value : '' }
      ];

      if (this.sort) {
          params.push({ key: Operators.sort, value: this.sort.active });
          params.push({ key: Operators.order, value: this.sort.direction });
      }

      return params.concat(this.additionalQueryParams)
          .filter(item => item.value !== undefined && item.value.toString().length);
    }

    /** Строит QueryString из @var additionalQueryParams. Исключает пустые параметры */
    private buildQueryString(): string {
        const params = [
            { key: '_page', value: (this.paginator.pageIndex + 1).toString() },
            { key: '_limit', value: (this.paginator.pageSize).toString() },
            { key: Operators.fullTextSearch, value: this.filter ? this.filter.nativeElement.value : '' }
        ];

        if (this.sort) {
            params.push({ key: Operators.sort, value: this.sort.active });
            params.push({ key: Operators.order, value: this.sort.direction });
        }

        return params.concat(this.additionalQueryParams)
            .filter(item => item.value !== undefined && item.value.toString().length)
            .map(item => `${item.key}=${item.value}`)
            .join('&');
    }
}
