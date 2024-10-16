import { Component, inject } from '@angular/core';
import { NumberService } from '../services/number.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-numbers',
  templateUrl: './numbers.component.html',
  styleUrl: './numbers.component.css'
})
export class NumbersComponent {
  service = inject(NumberService);
  numbers$ : Observable<number[]>;

  constructor(){
    this.numbers$ = this.service.getNumbers();
  }
}
