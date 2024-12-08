class BorospinceException(Exception):
    def __init__(self, message):
        super().__init__(message)

class Bor:
    def __init__(self, fajta, evjarat, alkoholtartalom=12.5):
        self.fajta = fajta
        self.evjarat = evjarat
        self.alkoholtartalom = alkoholtartalom  

    # Property for fajta
    @property
    def fajta(self):
        return self.__fajta
    
    @fajta.setter
    def fajta(self, value):
        self.__fajta = value

    # Property for evjarat
    @property
    def evjarat(self):
        return self.__evjarat
    
    @evjarat.setter
    def evjarat(self, value):
        self.__evjarat = value

    # Property for alkoholtartalom
    @property
    def alkoholtartalom(self):
        return self.__alkoholtartalom
    
    @alkoholtartalom.setter
    def alkoholtartalom(self, value):
        if not isinstance(value, (int, float)) or not (0 <= value <= 100):
            raise BorospinceException("Nem megfelelo alkoholtartalom!")
        self.__alkoholtartalom = value

    # Override the string representation
    def __str__(self):
        return f"{self.__fajta} (evjarat: {self.__evjarat}), melynek alkoholtartalma: {self.__alkoholtartalom}%"

    # Override equality operator
    def __eq__(self, other):
        if isinstance(other, Bor):
            return (
                self.__fajta.lower() == other.__fajta.lower() and
                self.__evjarat == other.__evjarat and
                self.__alkoholtartalom == other.__alkoholtartalom
            )
        return False

class Szekreny:
    def __init__(self):
        self.borok = []

    # Get wine by index
    def get_bor(self, n):
        if n < 0 or n >= len(self.borok):
            raise BorospinceException("Nem letezo index!")
        return self.borok[n]

    # Add wine with +=
    def __iadd__(self, bor):
        if isinstance(bor, Bor):
            self.borok.append(bor)
        else:
            raise TypeError("Nem bor!")
        return self

    # Combine cabinets with +
    def __add__(self, other):
        if not isinstance(other, Szekreny):
            raise TypeError("Nem szekreny!")
        combined = Szekreny()
        combined.borok = self.borok + other.borok
        return combined

    # Calculate average alcohol content
    def atlag_alkoholtartalom(self):
        if not self.borok:
            raise BorospinceException("Ures a szekreny!")
        total_alcohol = sum(bor.alkoholtartalom for bor in self.borok)
        return total_alcohol / len(self.borok)

    # Get wine statistics
    def statisztika(self):
        stats = {}
        for bor in self.borok:
            fajta = bor.fajta.lower()  # Case insensitive
            stats[fajta] = stats.get(fajta, 0) + 1
        return stats

    # Drink (remove) a specific wine
    def megisszak(self, bor):
        if not isinstance(bor, Bor):
            raise TypeError("Nem bor!")
        if bor not in self.borok:
            raise BorospinceException("Bor nem talalhato!")
        self.borok.remove(bor)

    # String representation of Szekreny
    def __str__(self):
        if not self.borok:
            return "Ez egy ures szekreny."
        stats = self.statisztika()
        return ", ".join(f"{count} {fajta}" for fajta, count in stats.items())

# Test script for Bor and Szekreny classes

# Import your Bor and Szekreny classes if they are in another file, or define them in this script.

try:
    # 1. Create Bor objects
    bor1 = Bor("Tokaji aszu", 2017, 13.5)
    bor2 = Bor("Egri bikaver", 2013, 12.0)
    bor3 = Bor("Chardonnay", 2019, 13.0)
    bor4 = Bor("Cabernet", 2018)  # default alcohol content (12.5)
    
    print("Bor objects created:")
    print(bor1)  # Tokaji aszu (evjarat: 2017), melynek alkoholtartalma: 13.5%
    print(bor2)  # Egri bikaver (evjarat: 2013), melynek alkoholtartalma: 12.0%
    print(bor3)  # Chardonnay (evjarat: 2019), melynek alkoholtartalma: 13.0%
    print(bor4)  # Cabernet (evjarat: 2018), melynek alkoholtartalma: 12.5%

    # 2. Test Bor comparison
    print("Comparing Bor objects:")
    print(bor1 == Bor("TOKAJI ASZU", 2017, 13.5))  # Should be True
    print(bor1 == bor2)                            # Should be False

    # 3. Create Szekreny objects
    szekreny1 = Szekreny()
    szekreny2 = Szekreny()
    
    # 4. Add Bor objects to Szekreny
    szekreny1 += bor1
    szekreny1 += bor2
    szekreny2 += bor3
    szekreny2 += bor4

    print("Szekreny 1 and 2 after adding wines:")
    print(szekreny1)  # Expected: '1 tokaji aszu, 1 egri bikaver'
    print(szekreny2)  # Expected: '1 chardonnay, 1 cabernet'

    # 5. Test combined cabinets with + operator
    szekreny3 = szekreny1 + szekreny2
    print("Combined Szekreny 3:")
    print(szekreny3)  # Expected: '1 tokaji aszu, 1 egri bikaver, 1 chardonnay, 1 cabernet'

    # 6. Test get_bor method
    print("Testing get_bor method:")
    print(szekreny3.get_bor(0))  # Expected: 'Tokaji aszu (evjarat: 2017), melynek alkoholtartalma: 13.5%'
    
    # 7. Test average alcohol content
    print("Average alcohol content in Szekreny 3:", szekreny3.atlag_alkoholtartalom())  # Should print the average of all added wines
    
    # 8. Test statisztika method
    print("Statistics in Szekreny 3:", szekreny3.statisztika())  # Expected: {'tokaji aszu': 1, 'egri bikaver': 1, 'chardonnay': 1, 'cabernet': 1}
    
    # 9. Test megisszak (remove) method
    szekreny3.megisszak(bor1)  # Remove bor1 from Szekreny 3
    print("Szekreny 3 after drinking Tokaji aszu:")
    print(szekreny3)  # Should not include 'tokaji aszu'

    # 10. Test exception handling by providing invalid inputs
    try:
        szekreny3 += "not a Bor object"  # This should raise a TypeError
    except TypeError as e:
        print(e)  # Expected: "Nem bor!"
    
    try:
        szekreny3.get_bor(-1)  # Invalid index
    except BorospinceException as e:
        print(e)  # Expected: "Nem letezo index!"

    try:
        szekreny3.atlag_alkoholtartalom()  # If szekreny is empty, should raise exception
    except BorospinceException as e:
        print(e)  # Expected: "Ures a szekreny!"

except Exception as e:
    print("An error occurred:", e)
