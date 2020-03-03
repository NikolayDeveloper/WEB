import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { NavigationStart, Router, RouterEvent } from '@angular/router';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from 'app/modules/column-config';
import { RequestDetails } from 'app/requests/models/request-details';
import { Subject } from 'rxjs/Rx';

import { ConfigService } from '../../../../../core';
import { RequestService } from '../../../../../requests/request.service';
import { ImageDataSource } from '../../../../../shared/image-data-source';
import { ImageDialog } from '../../../../image-dialog';
import { TrademarkDto } from '../../../../models/trademark-dto.model';
import { ExpertSearchSimilarService } from '../../../../services/expert-search-similar.service';
import { ImageServiceService } from '../../../../services/image-service.service';

const columnsConfigKey = 'image_search_expert_search_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
    { field: 'select', name: 'Выбрать', enabled: true },
    { field: 'barcode', name: 'Штрихкод', enabled: true },
    { field: 'requestTypeNameRu', name: 'Тип заявки', enabled: false },
    { field: 'statusNameRu', name: 'Статус', enabled: true },
    { field: 'previewImage', name: 'Изображение', enabled: true },
    { field: 'imageSimilarity', name: '% схожести (изображение)', enabled: true },
    { field: 'phonSimilarity', name: '% схожести (фонетика)', enabled: true },
    { field: 'semSimilarity', name: '% схожести (семантика)', enabled: true },
    { field: 'gosNumber', name: '№ ОД', enabled: true },
    { field: 'gosDate', name: 'Дата ОД', enabled: true },
    { field: 'regNumber', name: 'Рег. номер заявки', enabled: true },
    { field: 'regDate', name: 'Дата подачи заявки', enabled: true },
    { field: 'nameRu', name: 'Наименование на рус. яз.', enabled: true },
    { field: 'nameKz', name: 'Наименование на каз. яз.', enabled: false },
    { field: 'nameEn', name: 'Наименование на англ. яз.', enabled: false },
    { field: 'declarant', name: 'Заявитель', enabled: true },
    { field: 'owner', name: 'Владелец', enabled: true },
    { field: 'patentAttorney', name: 'Патентный поверенный', enabled: true },
    // { field: 'addressForCorrespondence', name: 'Адрес для переписки', enabled: true },
    { field: 'confidant', name: 'Доверенное лицо', enabled: true },
    { field: 'receiveTypeNameRu', name: 'Тип подачи заявки', enabled: true },
    { field: 'icgs', name: 'МКТУ', enabled: true },
    { field: 'icfems', name: 'МКИЭТЗ', enabled: true },
    { field: 'transliteration', name: 'Транслитерация', enabled: true },
    { field: 'priorityData', name: 'Приоритетные данные', enabled: true },
    { field: 'numberBulletin', name: 'Номер бюллетеня', enabled: true },
    { field: 'publicDate', name: 'Дата публикации', enabled: true },
    { field: 'validDate', name: 'Срок действия', enabled: true },
    { field: 'extensionDateTz', name: 'Дата продления', enabled: true },
    { field: 'disclaimerRu', name: 'Дискламация', enabled: true },
    { field: 'disclaimerKz', name: 'Дискламация каз', enabled: true }
];

@Component({
    selector: 'app-image-search-list',
    templateUrl: './image-search-list.component.html',
    styleUrls: ['./image-search-list.component.scss']
})
export class ImageSearchListComponent implements OnInit, AfterViewInit, OnDestroy {
    displayedColumns: string[];
    imageDataSource: ImageDataSource<TrademarkDto>;
    check: boolean;

    @Input() checkedTrademarkDtos: TrademarkDto[] = [];

    @Output() modifiedData = new EventEmitter<RequestDetails>();
    @Output() checkChanged = new EventEmitter<TrademarkDto[]>();

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    private onDestroy = new Subject();

    constructor(private fb: FormBuilder,
        private router: Router,
        private imageService: ImageServiceService,
        private configService: ConfigService,
        private columnConfigService: ColumnConfigService,
        public dialog: MatDialog,
        private requestService: RequestService) {
        this.router.events.subscribe((event: RouterEvent) => {
            if (event instanceof NavigationStart) {
                if (!event.url.includes('request')) {
                    this.columnConfigService.save(columnsConfigKey, defaultColumnsConfig);
                }
            }
        });
    }

    ngOnInit() {
        this.displayedColumns = this.columnConfigService.get(columnsConfigKey, defaultColumnsConfig)
            .filter(cc => cc.enabled)
            .map(cc => cc.field);
        this.initSearchTable();
        this.check = false;
    }

    ngOnDestroy(): void {
        this.onDestroy.next();
    }

    onColumnsChange() {
        const dialogRef = this.dialog.open(ColumnConfigDialogComponent, {
            data: {
                configKey: columnsConfigKey,
                defaultConfig: defaultColumnsConfig
            },
            disableClose: true
        });

        dialogRef.afterClosed()
            .takeUntil(this.onDestroy)
            .subscribe(result => {
                this.displayedColumns = result;
            });
    }

    ngAfterViewInit(): void {
        this.paginator.pageIndex = 0;
    }

    OnSearch(id: number) {
        this.imageService.searchByImage(id)
            .subscribe(results => this.imageDataSource.push(results));
    }

    stringifyICGS(array: string[]): any {
        if (array) {
            const sortedArray = array.sort((a, b) => a.localeCompare(b));

            return sortedArray.join('\n');
        } else {
            return array;
        }
    }

    onCheck(event) {
        if (event.checked) {
            this.check = true;
        } else {
            this.check = false;
        }
    }

    onSearchByNameAndImage(id: number) {
        this.imageService.searchByImageAndPhonetic(id)
            .subscribe(results => this.imageDataSource.push(results));
    }

    openImageDialog(url: string): void {
        this.dialog.open(ImageDialog, {
            data: url
        });
    }

    toggle(row: TrademarkDto) {
        if (!row) {
            return;
        }

        if (!this.checkedTrademarkDtos) {
            this.checkedTrademarkDtos = [];
        }

        if (this.checked(row)) {
            this.checkedTrademarkDtos = [...this.checkedTrademarkDtos.filter(r => r.ownerType !== row.ownerType
                || r.id !== row.id)];
        } else {
            this.checkedTrademarkDtos = [...this.checkedTrademarkDtos, row];
        }
    }

    checked(row: TrademarkDto): boolean {
        if (!(row && this.checkedTrademarkDtos)) {
            return false;
        }

        return this.checkedTrademarkDtos.some(r => r.ownerType === row.ownerType && r.id === row.id);
    }

    private getColumns(columnsConfig: ColumnConfig[]): string[] {
        return columnsConfig.filter(c => c.enabled).map(c => c.field);
    }

    private initSearchTable() {
        this.imageDataSource = new ImageDataSource<TrademarkDto>(this.sort, this.paginator);
    }
}
