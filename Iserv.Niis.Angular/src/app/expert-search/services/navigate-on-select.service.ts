import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class NavigateOnSelectService {
  selectedIndexRequestComponent = 0;
  selectedIndexExpertize = 2;
  constructor(
    private router: Router
  ) { }

  openItemInNewTab(moduleName: string, requestId: number, previosRequestId: number = null) {
    // localStorage.setItem('currentSelectedIndex', this.selectedIndexExpertize.toString());
    // localStorage.setItem('previosRequestId', previosRequestId.toString());
    // this.router.navigate([moduleName, requestId, this.selectedIndexRequestComponent]);
    window.open(`/${moduleName}/${requestId}`); // TODO открываем заявку в новом окне
  }
}
