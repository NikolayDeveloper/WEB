import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'app-details-popup',
    templateUrl: './details-popup.component.html',
    styleUrls: ['./details-popup.component.scss']
})
export class DetailsPopupComponent {
    constructor(@Inject(MAT_DIALOG_DATA) public data: any) {}

    get message() {
        if (this.data.message instanceof Array) {
            return this.data.message;
        } else {
            return [this.data.message];
        }
    }
}
