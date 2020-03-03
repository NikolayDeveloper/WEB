import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';

import { CreateDocumentDialogComponent } from '../../components/create-document-dialog/create-document-dialog.component';
import { JournalAutoAllocationComponent } from '../../components/journal-auto-allocation/journal-auto-allocation.component';


@Component({
  selector: 'app-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.scss']
})
export class JournalComponent implements OnInit {
  @ViewChild(JournalAutoAllocationComponent) autoAllocationComponent: JournalAutoAllocationComponent;
  constructor(public dialog: MatDialog,
    private router: Router) { }
  ngOnInit() {
  }

  onCreateDocumentClick() {
    this.dialog.open(CreateDocumentDialogComponent, {
      width: '500px',
      data: {},
    });
  }
  isTabAutoAllocation(): boolean {
    return this.router.url.indexOf('autoallocation') !== -1;
  }
}
