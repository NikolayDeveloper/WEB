import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { CreateBulletinSectionDialogComponent } from '../create-bulletin-section-dialog/create-bulletin-section-dialog.component';
import { Subject } from 'rxjs';
import { SectionsListComponent } from '../sections-list/sections-list.component';

@Component({
  selector: 'app-bulletin-sections',
  templateUrl: './bulletin-sections.component.html',
  styleUrls: ['./bulletin-sections.component.scss']
})
export class BulletinSectionsComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();
  @ViewChild(SectionsListComponent)
  private sectionsListComponent: SectionsListComponent;

  constructor(
    private dialog: MatDialog
  ) {
  }

  ngOnInit() {
  }

  onCreateBulletinSectionClick(): void {
    const config = new MatDialogConfig();
    config.width = '500px';
    config.disableClose = true;

    const dialogRef = this.dialog.open(CreateBulletinSectionDialogComponent, config);
    dialogRef.componentInstance.bulletinSectionAdded.subscribe(() => {
      dialogRef.componentInstance.bulletinSectionAdded.unsubscribe();
      this.sectionsListComponent.loadData();
    });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
