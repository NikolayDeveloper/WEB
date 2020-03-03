import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { PaymentsSearchParametersDto } from '../../models/payments-search-parameters.dto';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { DictionaryItem } from '../../helpers/dictionary-item';
import { PaymentsService } from '../../services/payments.service';

@Component({
  selector: 'app-payments-search',
  templateUrl: './payments-search.component.html',
  styleUrls: ['./payments-search.component.scss']
})
export class PaymentsSearchComponent implements OnInit {
  @Output() public applySearch: EventEmitter<PaymentsSearchParametersDto> = new EventEmitter();

  public searchParams = new PaymentsSearchParametersDto();
  public paymentStatuses: DictionaryItem[] = [];
  public currencies: string[];

  constructor(
    private dictionaryService: DictionaryService,
    private paymentsService: PaymentsService) {
  }

  public ngOnInit(): void {
    this.dictionaryService.getBaseDictionaryByCodes(DictionaryType.DicPaymentStatus, ['Distributed', 'NotDistributed', 'Returned'])
      .subscribe(result => {
        this.paymentStatuses = [];
        this.paymentStatuses.push(new DictionaryItem(null, null));
        this.paymentStatuses.push(...result.map(x => new DictionaryItem(x.id, x.nameRu)));
      });

    this.paymentsService.getCurrencies()
      .subscribe(result => {
        this.currencies = [];
        this.currencies.push(null);
        this.currencies.push(...result);
      });
  }

  public onApplySearch(): void {
    this.emitApplySearch();
  }

  public onResetSearch(): void {
    this.searchParams = new PaymentsSearchParametersDto();
    this.emitApplySearch();
  }

  private emitApplySearch(): void {
    const searchParams = new PaymentsSearchParametersDto();
    Object.assign(searchParams, this.searchParams);
    this.applySearch.emit(searchParams);
  }
}
