import { Component } from '@angular/core';

@Component({
  selector: 'app-404',
  template: `
  <div fxLayout="row" fxFlexFill fxLayoutAlign="center center">
    <mat-card class="mat-elevation-z6">
      <mat-card-content>
        <p class="title">404</p>
        <h3 i18n="@@pageNotFound">Страница не найдена</h3>
        <br>
      </mat-card-content>
      <mat-card-actions>
        <a mat-button routerLink="/" color="primary" i18n="@@textBack">Back</a>
      </mat-card-actions>
    </mat-card>
  </div>`,
  styles: [`
    .mat-card-content {
      min-width: 500px;
      text-align: center;
    }
    .title {
      margin: 0 auto;
      font-size: 10em;
    }`]
})
export class PageNotFoundComponent { }
