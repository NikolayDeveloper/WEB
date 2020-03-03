import { Component, OnInit, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
import { MaterialsService } from 'app/materials/services/materials.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-attachments',
  templateUrl: './attachments.component.html',
  styleUrls: ['./attachments.component.scss']
})
export class AttachmentsComponent implements OnInit, AfterViewInit {
  @Input() id: number;
  @Input() reloadAttachments: EventEmitter<any>;
  @Output() attachmentsLoad: EventEmitter<number> = new EventEmitter();

  attachments: any[];

  constructor(
    private materialsService: MaterialsService
  ) {}

  ngOnInit(): void {
    this.loadAttachments();
  }

  ngAfterViewInit(): void {
    if (this.reloadAttachments) {
      this.reloadAttachments.subscribe(() => {
        this.loadAttachments();
      });
    }
  }

  loadAttachments(): void {
    this.materialsService.getAllAttachments(this.id)
      .subscribe(attachments => {
        this.attachments = attachments;

        this.attachmentsLoad.emit(attachments.length);
      });
  }

  downloadAttachment(id: number): void {
    const attachment = this.attachments.find((entry: any) => (entry.id === id));

    this.materialsService.getAttachment(id)
      .subscribe(data => {
        saveAs(data, attachment.validName);
      });
  }
}
