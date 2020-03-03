import { Injectable } from '@angular/core';

@Injectable()
export class StateService {
  constructor() {}

  saveState(name: string, state: any = {}): void {
    localStorage.setItem(`state_${name}`, JSON.stringify(state));
  }

  loadState(name: string, needDelete: boolean = false): any {
    const state = localStorage.getItem(`state_${name}`);

    if (needDelete) {
      this.deleteState(name);
    }

    if (state) {
      return JSON.parse(state);
    } else {
      return null;
    }
  }

  deleteState(name: string): void {
    localStorage.removeItem(`state_${name}`);
  }
}
