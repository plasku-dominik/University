<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Student extends Model // az osztáyl azért Model, mert kiterjeszti a Model osztályt
{
    use HasFactory;

    // "bedródozott" adatokkal adjunk vissza egy hallgató listát 
    public function getList(){
        return [
            ['neptun' => 'AAABBB', 'name' => 'Kiss Erik'],
            ['neptun' => 'BBBAAA', 'name' => 'Nagy Anna']
        ];
    }
}
