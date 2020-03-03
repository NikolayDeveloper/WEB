import { Component, Input, Output, EventEmitter } from '@angular/core';
import { RequestDetails } from '../../../../../requests/models/request-details';
import { Router } from '@angular/router/src/router';
import { from } from 'rxjs/observable/from';
import { AfterViewInit, OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { ImageServiceService } from '../../../../services/image-service.service';

@Component({
    selector: 'app-image-search-form',
    templateUrl: './image-search-form.component.html',
    styleUrls: ['./image-search-form.component.scss']
})
export class ImageSearchFormComponent {
    @Input() requestId: number;
    @Input() imageUrl: string;
    @Output() search = new EventEmitter<any>();
    constructor(private imageServiceService: ImageServiceService) {

    }

    onSubmit() {
        this.search.emit(this.requestId);
    }
}
