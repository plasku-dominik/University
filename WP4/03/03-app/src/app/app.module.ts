import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CategoryModule } from './category/category.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NumbersComponent } from './numbers/numbers.component';

@NgModule({
  declarations: [
    AppComponent,
    NumbersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CategoryModule, // innentől kezdve a CategoryModule exportált elem...
    HttpClientModule // az alkalmazást http képessé teszem
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
