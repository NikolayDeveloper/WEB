import {
  Component,
  Input,
  OnDestroy,
  OnInit,
  OnChanges,
  SimpleChanges,
  Output,
  EventEmitter
} from '@angular/core';
import { MatSelectionList } from '@angular/material';
import { BibliographicDataService } from 'app/bibliographic-data/bibliographic-data.service';
import {
  ChangeTypeOption,
  ChangeTypeChooserComponent,
  ChangeType
} from 'app/bibliographic-data/models/changes-dto';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'app-change-type-choose',
  templateUrl: './change-type-choose.component.html',
  styleUrls: ['./change-type-choose.component.scss']
})
export class ChangeTypeChooseComponent
  implements OnInit, OnChanges, ChangeTypeChooserComponent, OnDestroy {
  @Input()
  details: IntellectualPropertyDetails;
  @Output()
  changed = new EventEmitter();
  types: ChangeTypeOption[];
  fullTypes: ChangeTypeOption[];
  formTypes: ChangeTypeOption[];
  selectedTypes: ChangeTypeOption[];
  private onDestroy = new Subject();

  constructor(private biblioService: BibliographicDataService) {}

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes.details && changes.details.currentValue) {
      this.biblioService
        .getChangeTypeOptions(this.details.ownerType, this.details.id)
        .takeUntil(this.onDestroy)
        .subscribe(data => {
          this.types = data;
          this.fullTypes = this.types.filter(t =>
            t.types.includes(ChangeType.Everything)
          );
          this.formTypes = this.types.filter(
            t => !t.types.includes(ChangeType.Everything)
          );
        });
    }
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSelect(list: MatSelectionList) {
    this.selectedTypes = this.types.filter(t =>
      list.selectedOptions.selected.map(s => s.value).includes(t.code)
    );
    this.changed.emit(this.selectedTypes.length);
  }

  getValue(): ChangeTypeOption[] {
    if (
      this.selectedTypes.some(st => st.types.includes(ChangeType.Everything))
    ) {
      return this.selectedTypes.filter(st =>
        st.types.includes(ChangeType.Everything)
      );
    }
    return this.selectedTypes;
  }
}
