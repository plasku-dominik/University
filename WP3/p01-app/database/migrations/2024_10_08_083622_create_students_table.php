<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Adott lépésben leírom, hogy mi a feladat az adatbázisstruktrúával
     */
    public function up(): void
    {
        // Schema::create -> tábla létrehozás
        //   1) a tábla neve (students)
        //   2) closure függvény, amely paraméterben kapja a létrehozott tábla munkapéldányát
        //              --> leírom, hogy mit kell csinálni a táblán belül
        Schema::create('students', function (Blueprint $table) {
            // a mezők létrehozás a $table metódushívásain keresztül történik meg
            //   -> egy hívás - 1 mező létrehoása, és a paraméterekben leírjuk az elemeket
            $table->id(); // hozz létre egy bigint típusú auto incremenet mezőt, ami legyen PK
            $table->char('neptun', 6)->nullable(false); // neptun CHAR(6) NOT NULL
            $table->string('name', 200)->nullable(false); // name VARCHAR(200) NOT NULL
            $table->timestamps(); // a lérehozási és módosítási idő automatikus kezelése

            // egyéb megszorítások
            $table->unique('neptun'); // unique megszorítás (ha több mező együttesen értintett akkor szöveg tömbben adom át )
        });
    }

    /**
     * Megmondom, hogy hogyan kell visszavonni az up metódusban leírt feladatot (inverz)
     */
    public function down(): void
    {   // egy lépésban a Schema::dropIfExists-el eldobom a paraméterben átadott a táblát, ha létezik
        Schema::dropIfExists('students');
    }
};
