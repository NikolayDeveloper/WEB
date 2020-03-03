import {Component, Input, OnInit, OnChanges, ViewChild, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit, OnChanges {
  @Input() position: string;
  @Input() mode: string;
  @Input() state: boolean;
  @Input() width: string;
  @Input() bgc: string;
  @Output() closed = new EventEmitter<boolean>();
  @ViewChild('sidenav') sidenav: any;

  constructor() { }
  ngOnInit() {
  }
  ngOnChanges() {
  }

  onClose() {
    this.closed.emit(false);
  }

}
