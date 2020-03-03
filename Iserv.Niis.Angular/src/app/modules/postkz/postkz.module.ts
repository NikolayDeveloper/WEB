import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { PostKzService } from './postkz.service';


@NgModule({
  imports: [
    HttpClientModule
  ],
  providers: [
    PostKzService
  ]
})
export class PostkzModule { }
