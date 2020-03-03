import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
// tslint:disable-next-line:import-blacklist
import { Subject } from 'rxjs';
import { DocumentCommentDto } from '../../../materials/models/materials.model';
import { Config } from '../table/config.model';
import { FormBuilder, FormGroup, NG_VALUE_ACCESSOR } from '@angular/forms';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { AuthenticationService } from '../../../shared/authentication/authentication.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent implements OnInit, OnChanges, OnDestroy {

  stateChanges = new Subject<void>();
  private _disabled = false;
  private onDestroy = new Subject();
  columns: Config[] = [
    new Config({ columnDef: 'comment', header: 'Комментарий', class: 'width-200' }),
    new Config({ columnDef: 'authorInitials', header: 'Автор', class: 'width-200' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата', class: 'width-200' })
  ];
  formGroup: FormGroup;
  tableComments: DocumentCommentDto[];
  newCommnets: DocumentCommentDto[];

  @Input()
  get comments(): DocumentCommentDto[] {
    return this.tableComments;
  }

  set comments(value: DocumentCommentDto[]) {
    this.tableComments = value;
    this.stateChanges.next();
  }

  @Input() documentId: number[];
  @Output() changed = new EventEmitter<DocumentCommentDto>();
  @Input()
  get disabled() {
    return this._disabled;
  }
  set disabled(value) {
    this._disabled = coerceBooleanProperty(value);
    this.stateChanges.next();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.disabled) {
      this.setDisabledState(changes.disabled.currentValue);
    }
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    isDisabled
      ? this.formGroup.get('comment').disable()
      : this.formGroup.get('comment').enable();
  }

  constructor(
    private authenticationService: AuthenticationService,
    private fb: FormBuilder) {

    this.tableComments = [];
    this.newCommnets = [];

    this.buildForm();
  }

  ngOnInit() {
    this.setDisabledState(this.disabled);
  }

  ngOnDestroy(): void {
    this.stateChanges.complete();
    this.onDestroy.next();
  }

  onClick() {
    const value = this.formGroup.get('comment').value;

    if (value === null || value === '') {
      return;
    }

    this.addComment(value);
  }

  onKeydown(event: any) {
    if (event.key === 'Enter') {

      if (event.target.value === null || event.target.value === '') {
        return;
      }

      this.addComment(event.target.value);
    }
  }

  addComment(value: string) {
    const comment = new DocumentCommentDto();
    comment.authorId = this.authenticationService.userId;
    comment.authorInitials = this.authenticationService.name;
    comment.comment = value;
    comment.dateCreate = new Date(Date.now());
    comment.isDeleted = false;

    this.tableComments = [...this.tableComments, comment];
    this.newCommnets = [...this.newCommnets, comment];

    this.changed.emit(comment);

    this.formGroup.get('comment').setValue('');
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      comment: ['']
    });
  }
}
