<!-- cél az alap keret megteremtése, amibe minden tartalmat beágyazunk -->
 <!DOCTYPE html>
 <html lang="en">
 <head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>???</title>
 </head>
 <body>
    <header>
        <!-- az oldal fejlécének kifejtése -->
        <!-- include: egy másik nézetfájl helyének betöltése az adott ponton -->
        @include('public.main_header')
    </header>
    <nav>
        <!-- leírom az oldal statikus menürendeszerét -->
        <!-- kiszervezeem a main_menu fájlba a menü leírását,
             és betöltöm ide -->
        @include('public.main_menu')
    </nav>
    <!-- ide jön majd az a tartalom, ami a konkrét nézettől függ 
       -> elhelyezek egy helyőrzőt, amely tartalma a későbbiekben felülírható
    -->
    @yield('dynamic_content')
 </body>
 </html>