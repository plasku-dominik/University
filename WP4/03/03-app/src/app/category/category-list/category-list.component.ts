import { Component, inject } from '@angular/core';
import { Category } from '../../model/category';
import { CategoryService } from '../../services/category.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.css'
})
export class CategoryListComponent {
  // azokat az elemeket, amelyeket a nézet készítése során el szeretnénk érni felveszem mezőként/metódusként
  // !!! a nézetből az osztály minden nem private eleme elérhető lesz
  records : Category[] = [
    { name: 'Beauty', url: 'beauty'},
    { name: 'Furniture', url: 'furniture'}
  ];

  // konvencionálisan az aszinkron mezők neve $ jellel végződik
  records2$ : Observable<Category[]>;

  // felveszek egy adatmezőt, amiben a szolgáltatásra vonatkozó referenciát tárolom
  // fogom tárolni, meghatározom a típust is
  categoryService1 : CategoryService = inject(CategoryService);

  // v2) az osztály konstruktorában kezdeményezem a beszűrást
  // a paraméternek meghatározom a védelmi szintjét is, így az angular nem a paramétert
  // fogadja csak, hanem abból lokális adatmezőt készít
  constructor(private categoryService2 : CategoryService){
    // v1) a hívás eredménye egy observable típusú elem, amelyre fel tudunk iratkozni
    // -> értesítést kapok arról, ha sikeresen vagy hibával befejeződött a művelet
    this.categoryService1.load().subscribe({
      next: (data) => this.records = data,
      error: (error) => console.log(error),
      complete: () => console.log('complete')
    })

    // v2) követlenül tárolom az aszinkron adatfolyamot
    this.records2$ = this.categoryService1.load();
  }

  // feladata, hogy a gombra kattintásnál meghívjuk és törölje az összes elemet a recordnak
  deleteAll(){
    this.records = []; // újradeklarálom a tömböt, így az elemek
  }
}
