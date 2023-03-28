Készítsen egy CarDb nevű LocalDB adatbázist.

Kérje le a connection string-jét, és állítsa be, hogy a program indításakor mindig másolódjon be az adatbázis jelenlegi (üres) állapota a kimeneti könyvtárba.

Code-first megközelítéssel készítse el az alábbiakat:

0.) A főprogram váza így néz ki:

class Program
    {
        static void Main(string[] args)
        {
            CarDbContext ctx = new CarDbContext();
            ctx.Brand.Select(x => x.Name).ToConsole("BRANDS");
            
        }
    }

Ehhez valósítsuk meg:
- a ToConsole kiterjesztését az IEnumerable<T típusnak>
- az egész végén készítsünk el 3 lékérdezést:
a.) Az összes márkát irassuk ki
b.) Az összes modelt irassuk ki
c.) Az Átlagos BasePrice-t irassuk ki márkánként


1.) Egy Brand táblát reprezentáló osztályt, amely tartalmaz:
- egy Id nevű intet, ami elsődleges kulcs és aminek az értéke automatikusan növekszik az insertekkel
- egy legfeljebb 100 hosszúságú stringet, melynek megadása kötelező,
és a neve az, hogy Name
- egy olyan Cars nevű ICollection<Car> típusú tagváltozót, amely nem kerül bele az adatbázisba, de az egy-a-többhöz virtuális mappeléshez használható
- egy default konstruktort, amely HashSet konkrét típussal példányosítja az üres Cars tagváltozót

2.) Egy Cars táblát reprezentáló osztályt, melynek neve Car, de egy "cars" nevű táblát reprezentál. Az osztály tartalmaz:
- egy Id nevű intet, ami elsődleges kulcs, aminek az értéke automatikusan növekszik az insertekkel, és amelynek adatbázisban levő oszlopneve az, hogy "car_id"
- egy legfeljebb 100 hosszúságú stringet, melynek megadása kötelező, és a neve az, hogy Model
- egy BasePrice nevű, int típusú adattagot, melynek megadása nem kötelező
- egy virtuális Brand típusú tagváltozót, amely az adatbázisban nem kap helyet, azonban egy egy-a-többhöz mappeléshez használható.
- egy BrandId nevű int típusú idegen kulcsot a Brand táblára

3.) Egy DbContext-et kiterjesztő CarDbContext nevű osztályt, amely tartalmaz:
- Egy DbSet<Brand> és egy DbSet<Car> típusú virtuális változót Brand és Cars néven, hivatkozásul a táblákra
- Default konstruktort, melyben garantáljuk, hogy a this.Database létrejött
- Egy saját override-olt OnConfiguring() metódust, melyben a LazyLoading be van kapcsolva és SqlServer kapcsolat string be van állítva
- Egy saját override-olt OnModelCreating() metódust, melyben beállítjuk, hogy:
... a) Egy Car objektumhoz egy Brand tartozik, amely több Car-nak is lehet a brandje. Beállítjuk az idegen kujlcsot is (BrandId), törléskor pedig a ClientSetNull opciót használjuk
... b) Az adatbázisban az alábbi értékek szerepeljenek:

Brand bmw = new Brand() { Id = 1, Name = "BMW" };
Brand citroen = new Brand() { Id = 2, Name = "Citroen" };
Brand audi = new Brand() { Id = 3, Name = "Audi" };

Car bmw1 = new Car() { Id = 1, BrandId = bmw.Id, BasePrice = 20000, Model = "BMW 116d" };
Car bmw2 = new Car() { Id = 2, BrandId = bmw.Id, BasePrice = 30000, Model = "BMW 510" };
Car citroen1 = new Car() { Id = 3, BrandId = citroen.Id, BasePrice = 10000, Model = "Citroen C1" };
Car citroen2 = new Car() { Id = 4, BrandId = citroen.Id, BasePrice = 15000, Model = "Citroen C3" };
Car audi1 = new Car() { Id = 5, BrandId = audi.Id, BasePrice = 20000, Model = "Audi A3" };
Car audi2 = new Car() { Id = 6, BrandId = audi.Id, BasePrice = 25000, Model = "Audi A4" };

modelBuilder.Entity<Brand>().HasData(bmw, citroen, audi);
modelBuilder.Entity<Car>().HasData(bmw1, bmw2, citroen1, citroen2, audi1, audi2);