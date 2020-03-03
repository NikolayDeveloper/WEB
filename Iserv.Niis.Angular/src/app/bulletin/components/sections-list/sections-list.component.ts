import { Component, OnInit, OnDestroy } from '@angular/core';
import { Config } from 'app/shared/components/table/config.model';
import { BulletinSectionsService } from 'app/bulletin/services/bulletin-sections.service';
import { BulletinSectionListDto } from 'app/bulletin/models/bulletin-section-list-dto';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { EditBulletinSectionDialogComponent } from '../edit-bulletin-section-dialog/edit-bulletin-section-dialog.component';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sections-list',
  templateUrl: './sections-list.component.html',
  styleUrls: ['./sections-list.component.scss']
})
export class SectionsListComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();

  public readonly columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({ columnDef: 'name', header: 'Наименование раздела', class: 'width-200' }),
    new Config({
      columnDef: 'editBulletinSection',
      header: 'Редактировать раздел бюллетеня',
      class: 'width-25 sticky',
      icon: 'edit',
      click: row => this.onEditBulletinSectionClick.call(this, row),
      disable: (row: BulletinSectionListDto) => false
    })
  ];
  public readonly reset = new Subject<any[]>();

  constructor(
    private router: Router,
    public dialog: MatDialog,
    public bulletinSectionsService: BulletinSectionsService
  ) {
  }

  ngOnInit() {
  }

  public loadData(): void {
    this.reset.next([]);
  }

  public onSelect(bulletinSectionListDto: BulletinSectionListDto): void {
    this.router.navigate(['bulletin', 'sections', bulletinSectionListDto.id]);
  }

  private onEditBulletinSectionClick(row: BulletinSectionListDto): void {
    const config = new MatDialogConfig();
    config.width = '500px';
    config.disableClose = true;
    config.data = {
      BulletinSectionListDto: row
    };

    const dialogRef = this.dialog.open(EditBulletinSectionDialogComponent, config);
    dialogRef.componentInstance.bulletinSectionEdited.subscribe(() => {
      dialogRef.componentInstance.bulletinSectionEdited.unsubscribe();
      this.loadData();
    });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
