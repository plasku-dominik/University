import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CategoriesListComponent } from './category/categories-list/categories-list.component';
import { CategoryModule } from './category/category.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CategoryModule // importálom a modult, így az ebben a modulban deklarált és exportált elemekre már hivatkozhatok
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
