import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { Subject } from 'rxjs';
import { CreateBulletinDialogComponent } from '../create-bulletin-dialog/create-bulletin-dialog.component';

@Component({
  selector: 'app-bulletin-section-details',
  templateUrl: './bulletin-section-details.component.html',
  styleUrls: ['./bulletin-section-details.component.scss']
})
export class BulletinSectionDetailsComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();

  constructor(
    private dialog: MatDialog
  ) {
  }

  ngOnInit() {
  }

  onCreateBulletinClick(): void {
    const config = new MatDialogConfig();
    config.width = '500px';
    config.disableClose = true;

    const dialogRef = this.dialog.open(CreateBulletinDialogComponent, config);
    /*
    dialogRef.componentInstance.bulletinSectionAdded.subscribe(() => {
      dialogRef.componentInstance.bulletinSectionAdded.unsubscribe();
      this.sectionsListComponent.loadData();
    });
    */

    /*
    const dialogRef = this.dialog.open(CreateBulletinSectionDialogComponent, config);
    dialogRef.componentInstance.bulletinSectionAdded.subscribe(() => {
      dialogRef.componentInstance.bulletinSectionAdded.unsubscribe();
      this.sectionsListComponent.loadData();
    });
    */
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
