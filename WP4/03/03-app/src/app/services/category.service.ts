import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Category } from '../model/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  // felkészülök arra, hogy http szolgáltatásokat tudjak hívni
  // -> injektálom az ehhez szükséges belső szolgáltatást
  private httpService : HttpClient = inject(HttpClient);
  constructor() { }

  log() { console.log('CategoryService');}

  // kategóriák letöltése a szerverről
  load() {
    const API_URL = 'https://dummyjson.com/products/categories';
    // a httpClient minden http metódushoz kiajánl egy saját -> azt a metódust kell meghívni, ahogy kommunkikálni
    // szeretnénk a szerverrel (http metódus)
    return this.httpService.get<Category[]>(API_URL);
  }
}
