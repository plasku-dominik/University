<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Student; // betöltöm a sutdent osztály a megfelelő névtérből

// a http kérésben jövő path azonosítója a kontrollert (/hallgatok -> StudentController)
class StudentController extends Controller // A Contollerek kiterjesztik az Controller őosztályt
{
    // a http kérésében érkező metódus meghatározza, hogy melyik metódust kell
    // az osztályból végrehajtani
    /*
        add() -> POST
        delete() -> DELETE
        update() -> PUT
        list() -> GET
        show() -> GET (~ a további paraméterek segítik a konkrét döntést)

    */

    // A kontrollerben közvetlenül meghívható feladatokat publikus metódusként definiáljuk
    public function add(){ // student hozzáadása
        return view('student.add');
    }
    // validálom az adatokat és ha minden renben van, akkor feldoglozom azokat -> eltárolom a rekordot
    public function store(Request $request){ // Dependency Injection-nel elkérem a konkrét kéréshez tartozó leírást (benne az adatokkal)
        // validáljuk az adatokat -> ha legalább egy validáción megbukunk, akkor a laravel visszairányít az űrlapra
        // egy tömbben átadom az egyes mezőkhöz a validációs szabályok sorát, ha minden rendben van, akkor továbbenged a következő sorra
        // ,ha legalább 1 validációs hiba felmerül, akkor visszapattan...
        $request->validate([
            'neptun_code' => 'required',
            'stud_name' => 'required'
        ]);
    }

    public function delete(){ // student törlése
        echo 'töröl';
    }
    public function update(){ // student módosítása
        echo 'frissít';
    }

    /* múlt
    public function list(){ // students lista
        // nézetet felhelyezni a view függvénnyel tudunk,
        // amely kiindulási pontként a resources/views
        // almappát jelöli ki, az első paraméterben
        // ezen a mappán belül szövegesen megadjuk
        // a fájl elérési útját (a elválasztó a .,
        // a fájl esetén nem írjuk ki a blade.php végződést)
        // a nézetet a válaszban szeretnénk visszaadni,
        // ezért returnolni kell a hívás eredményét

        $studentModel = new Student(); // létrehozom a példányt
        $students = $studentModel->getList(); // meghívom a metódust
        // a nézet csak azokhoz az adatokhoz fér hozzá, amit át adunk neki
        // ehhez a view második paraméterében egy asszociatív tömböt tudunk
        // átadni, és a nézetben a tömb kulcsai lesznek a változó nevek
        return view('student.list', ['records' => $students]); // habár a változót a controllerben $students-nek hívom, a nézet számára átnevezem $records-á
        // !! egy nézetet adhatunk vissza, tehát
        // több return esetén az első eredményét kapja
        // meg a bögnésző


    }*/

    // jelen + jövő
    public function list(){
        $students = Student::all(); // lekérdezem a modellen keresztül a tábla összes mezőjét + rekordját
        return view('student.list', ['records' => $students]); // felhelyezem a nézetet, átadva a rekordokat
    }
    public function show(){ // egy student részletes mutatása
        echo 'részletes nézet';
    }

}
