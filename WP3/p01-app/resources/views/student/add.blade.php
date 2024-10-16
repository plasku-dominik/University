@extends('public.main_layout')
@section('dynamic_content')
<h2>Új hallgató rögzítse</h2>
<form method="POST" action="{{route('stud.store')}}">
    @csrf
    <input type="text" name="neptun_code" placeholder="Neptun kód"><br>
    <input type="text" name="stud_name" placeholder="Hallgató neve"><br>
    <button type="submit">Mentés</button>
</form>
@endsection
