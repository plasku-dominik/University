<!-- 1) megöröklöm a main_layout tartalmát, így annak tartalmát láthatom -->
@extends('public.main_layout')

<!-- 2) a megörökölt nézet yield elemeihez saját tartalmat rendelek a section
        direktíva segítségével -->
@section('dynamic_content')
<h2>Hallgatók listája</h2>
<!-- a) ha nincs egyetlen hallgató sem, akkor kiírom, hogy sajnos még nincs 
        hallgató, különben táblázatos formában megjelenítem a hallgatókat -->
<p>
    <!-- a nézetben a php hivatkozásokat (változók, kifejezések ) dupla kapcsos zárójelbe kell tenni }} -->
    Hallgatók száma: {{count($records)}}
</p>
@endsection