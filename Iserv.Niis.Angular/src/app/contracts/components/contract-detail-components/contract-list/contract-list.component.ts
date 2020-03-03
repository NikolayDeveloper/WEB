import {
  Component,
  OnInit,
  Input,
  OnDestroy,
  OnChanges,
  SimpleChanges
} from '@angular/core';
import { Config } from '../../../../shared/components/table/config.model';
import { Router } from '@angular/router';
import { ContractService } from '../../../contract.service';
import { ContractItem } from '../../../models/contract-item';
import { OwnerType } from '../../../../shared/services/models/owner-type.enum';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-contract-list',
  templateUrl: './contract-list.component.html',
  styleUrls: ['./contract-list.component.scss']
})
export class ContractListComponent implements OnInit, OnDestroy, OnChanges {
  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({
      columnDef: 'contractNum',
      header: 'Регистрационный номер заявления на договор',
      class: 'width-150'
    }),
    new Config({
      columnDef: 'regDate',
      header: 'Дата подачи Заявления на регистрацию договора',
      class: 'width-150'
    }),
    new Config({
      columnDef: 'gosNumber',
      header: 'Регистрационный номер договора',
      class: 'width-150'
    }),
    new Config({ columnDef: 'gosDate', header: 'Дата регистрации договора', class: 'width-100' }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-100' }),
    new Config({ columnDef: 'initiator', header: 'Пользователь', class: 'width-100' }),
    new Config({ columnDef: 'executor', header: 'Исполнитель', class: 'width-100' }),
    new Config({ columnDef: 'statusNameRu', header: 'Статус', class: 'width-100' }),
    new Config({ columnDef: 'typeNameRu', header: 'Вид договора', class: 'width-100' }),
    new Config({ columnDef: 'categoryNameRu', header: 'Категория договора', class: 'width-100' }),
    new Config({ columnDef: 'sideOneNameRu', header: 'Сторона 1', class: 'width-100' }),
    new Config({ columnDef: 'sideTwoNameRu', header: 'Сторона 2', class: 'width-100' }),
    new Config({ columnDef: 'validDate', header: 'Срок действия договора', class: 'width-100' }),
  ];
  contracts: ContractItem[] = [];
  private onDestroy = new Subject();

  @Input() ownerId: number;
  @Input() ownerType: OwnerType;
  @Input() disabled: boolean;
  constructor(
    private router: Router,
    private contractService: ContractService
  ) {}

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.ownerType && changes.ownerId) {
      this.getContractsByOwner();
    }
  }

  private getContractsByOwner() {
    this.contractService
      .getByOwner(this.ownerId, this.ownerType)
      .takeUntil(this.onDestroy)
      .subscribe(contracts => (this.contracts = contracts));
  }

  ngOnDestroy(): void {
    this.contracts = [];
    this.onDestroy.next();
  }

  onSelect(record: ContractItem) {
    this.router.navigate([`contracts`, record.id]);
  }
}
