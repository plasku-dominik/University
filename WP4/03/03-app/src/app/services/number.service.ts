import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NumberService {

  constructor() { }

  getNumbers() : Observable<number[]>{
    let numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    //return numbers; // szinkron adatfolyam
    return of(numbers); // aszinkron adatfolyam
  }
}
