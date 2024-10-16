<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Student extends Model // azért vagyok model, mert kiterjesztem a model osztályt
{
    use HasFactory;

    // 1) bellátom az osztály azokkal az információkkal, hogy a kapcsoalt felépíthető legyen a táblával
    //  a) az osztály neve + s adja meg a tábla nevét (snak-case írásmód): Student -> students
    //    explicit megadás:
    protected $table = 'students';

    //  b) id-nek hívják az elsődleges kulcsot, ami AI és numerikus
    // explicit megadás:
    protected $primaryKey = 'id';
    protected $keyType = 'bigint';
    public $incremented = true;
}
