<?php

// forgalomirányítás: összerendeli a http kéréseket és controller/metódus párokat

// Route osztályon keresztül veszek fel új útvonalat
// a Route-nak a metódusai egybeesnek a http metódusokkal
// Route::get -> GET http metódu
// Route::post -> POST http metódu
// Route::delete -> DELETE http metódu
// ...

use Illuminate\Support\Facades\Route;
use App\Http\Controllers\StudentController; // a StudentController betöltése a névtérből

Route::get('/', function () {
    return view('welcome');
});


// /hallgatok + GET kérés -> StudentController / list metódus
Route::get('/hallgatok', [StudentController::class, 'list']);

// adat rögzítés: 1) get-es kérésre mutatok egy űrlapot, 2) post kérésre feldolgozom a beküldött adatokat
Route::get('/hallgatok/uj', [StudentController::class, 'add']);
// az útvonalakhoz neveket tudok rendelni és később a működés során az útvonalakat névvel tudjuk hivatkozni
Route::post('/hallgatok', [StudentController::class, 'store'])->name('stud.store'); // POST -> add


Route::put('/hallgatok', [StudentController::class, 'update']); // PUT -> update
Route::delete('/hallgatok', [StudentController::class, 'delete']); // DELETE -> delete
