# =====================================================================================================================
# Python - Objektumorientáltság, kivételkezelés (Egy lehetséges megoldás)
# =====================================================================================================================

# === 1. feladat: A BorospinceException osztály ===

class BorospinceException(Exception):   # Egy saját kivételosztály, amely az Exception osztályból származik
    def __init__(self, hibauzenet):
        super().__init__(hibauzenet)    # Meghívjuk az ősosztály konstruktorát, és átadjuk paraméterben a hibaüzenetet


# === 2. feladat: A Bor osztály ===

class Bor:
    # Konstruktor

    def __init__(self, fajta, evjarat, alkohol=12.5):   # Az `alkohol` paraméter megadását opcionálissá tesszük
        self.__fajta = fajta                             # Az adattagokat a paraméterek alapján inicializáljuk
        self.__evjarat = evjarat
        self.alkoholtartalom = alkohol                  # A setterrel ellenőrzött módon állítjuk be az adattag értékét

    # Get és set property-k

    @property
    def fajta(self):
        return self.__fajta

    @fajta.setter
    def fajta(self, ertek):
        self.__fajta = ertek

    @property
    def evjarat(self):
        return self.__evjarat

    @evjarat.setter
    def evjarat(self, ertek):
        self.__evjarat = ertek

    @property
    def alkoholtartalom(self):
        return self.__alkoholtartalom

    @alkoholtartalom.setter
    def alkoholtartalom(self, ertek):
        if isinstance(ertek, (float)) and 0 <= ertek <= 100:      # Ha a paraméter egy 0 és 100 közötti valós szám...
            self.__alkoholtartalom = ertek                       # ...beállítjuk az adattag értékét a paraméterre...
        else:                                                   # ...egyébként pedig dobunk egy kivételt
            raise BorospinceException("Nem megfelelo alkoholtartalom!")

    # Az objektum szöveggé alakításáért felelő metódus

    def __str__(self):
        return f"{self.__fajta} (evjarat: {self.__evjarat}), melynek alkoholtartalma: {self.__alkoholtartalom}%"

    # Operator overloading

    def __eq__(self, bor):
        if not isinstance(bor, Bor):
            return False

        return self.fajta.lower() == bor.fajta.lower() and self.evjarat == bor.evjarat and self.alkoholtartalom == bor.alkoholtartalom


# === 3. feladat: A Szekreny osztály ===

class Szekreny:
    # Konstruktor

    def __init__(self):
        self.borok = []

    # Az objektum szöveggé alakításáért felelő metódus

    def __str__(self):
        if len(self.borok) == 0:                # Üres szekrény esetének lekezelése
            return "Ez egy ures szekreny."

        eredmeny = ""                           # Ebben a változóban állítjuk elő a visszaadott szöveget
        stat = self.statisztika()               # Felhasználjuk a `statisztika` függvény által visszaadott dictionary-t

        for fajta, mennyiseg in stat.items():   # A statisztikákat tartalmazó dictionary bejárása...
            eredmeny += f"{mennyiseg} {fajta}, "

        eredmeny = eredmeny[:-2]                # Eltávolítjuk az eredmény szöveg végén lévő vesszőt és szóközt
        return eredmeny

    # Operator overloading

    def __iadd__(self, bor):                        # A `+=` operátor felüldefiniálása
        if not isinstance(bor, Bor):                # Nem Bor típusú paraméter esetén kivételt dobunk
            raise TypeError("Nem bor!")

        self.borok.append(bor)                      # Ha Bort kapunk paraméterben, felvesszük a `borok` listába
        return self                                 # Visszatérünk az aktuális objektummal

    def __add__(self, szekreny):                    # A `+` operátor felüldefiniálása
        if not isinstance(szekreny, Szekreny):      # Nem Szekrény típusú paraméter esetén kivételt dobunk
            raise TypeError("Nem szekreny!")

        # Ha megfelelő a paraméter típusa, akkor létrehozunk egy új Szekreny objektumot, aminek a `borok` listája a két
        # összeadott szekrény `borok` listájának összefűzése lesz

        uj_szekreny = Szekreny()
        uj_szekreny.borok = self.borok + szekreny.borok     # Két listát egyszerűen összefűzhetünk a `+` operátorral
        return uj_szekreny

    # Egyéb metódusok

    def get_bor(self, n):
        if n < 0 or n >= len(self.borok):               # Ha nem megfelelő az index értéke, kivételt dobunk...
            raise BorospinceException("Nem letezo index!")

        return self.borok[n]                            # ...egyébként visszaadjuk a `borok` lista adott indexű elemét

    def atlag_alkoholtartalom(self):
        if len(self.borok) == 0:                        # Ha nincs egyetlen bor se a szekrényen, kivételt dobunk...
            raise BorospinceException("Ures a szekreny!")

        # ...egyéb esetben visszaadjuk a szekrényen található borok alkoholtartalmának átlagát

        osszeg = 0
        darabszam = len(self.borok)

        for bor in self.borok:
            osszeg += bor.alkoholtartalom

        return osszeg / darabszam

    def statisztika(self):
        stat = {}           # Nem tudjuk előre, hogy milyen boraink vannak, ezért egy üres dictionary-ből indulunk ki

        for bor in self.borok:                  # Végigmegyünk a szekrényen található borokon
            fajta = bor.fajta.lower()           # A bor fajtájának nevében a kis- és nagybetűket nem különböztetjük meg

            if fajta not in stat:               # Ha még nem láttuk korábban az adott fajtanevet...
                stat[fajta] = 1                 # ...felvesszük 1-es előfordulási értékkel
            else:                               # Ha pedig már láttuk korábban az adott fajtanevet...
                stat[fajta] += 1                # ...növeljük a hozzá tartozó előfordulási értéket

        return stat         # Visszatérünk a statisztikát tároló dictionary-vel

    def megisszak(self, bor):
        if not isinstance(bor, Bor):            # Nem Bor típusú paraméter esetén kivételt dobunk
            raise TypeError("Nem bor!")

        if bor not in self.borok:               # A szekrényen nem megtalálható borok esetén kivételt dobunk
            raise BorospinceException("Bor nem talalhato!")

        self.borok.remove(bor)                  # Töröljük a `borok` listából a paraméterben kapott bort


# === A főprogram ===

if __name__ == '__main__':
    # Itt tesztelhetjük a megírt osztályok és metódusok működését...

    pass
