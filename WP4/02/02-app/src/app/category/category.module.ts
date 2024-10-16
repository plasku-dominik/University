import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesListComponent } from './categories-list/categories-list.component';



@NgModule({
  declarations: [
    CategoriesListComponent // ez a komponens a CategoryModulhoz tartozik
  ],
  exports: [ // felsorolom a delarált elemek közül azokat, amelyeket is szeretném, hogy elérjenek és közvetlenül használjanak
    CategoriesListComponent
  ],
  imports: [
    CommonModule // alapértelmezés szerint hozzáadja, ez a legfontoabb direktívákat és Angular elemeket tartalmazza
  ]
})
export class CategoryModule { }
