# A feladatot készítette: Gáspár Balázs (2021/2022/2-es félév)

## Általános leírás

A mellékelt "02_war_of_westeros.xml" fájl a Trónok harca sorozat háborús helyzetét tartalmazza az
alábbiakban írt formában. A fájlban nem mindenhol szerepel az összes lenti adat. (Értsd: lehetnek
"hiányzó" node-ok az XML-ben.)

## XML adatok

Egy-egy csatáról az alábbiakat lehet tudni:

- csata neve
- mikor történt
- ki nyert
- milyen típusú csata
- hány híres szereplő halt meg
- hány híres szereplő lett fogoly
- évszak
- hely
- régió
- támadó sereg
- védekező sereg

```xml
<battle>
  <name>Battle of the Golden Tooth</name>
  <year>298</year>
  <outcome>attacker</outcome>
  <type>pitched battle</type>
  <majordeath>1</majordeath>
  <majorcapture>0</majorcapture>
  <season>summer</season>
  <location>Golden Tooth</location>
  <region>The Westerlands</region>
  <attacker>
    ...
  </attacker>
    ...
  </defender>
</battle>
```

Egy-egy seregről pedig az alábbiakat:

- király
- parancsnokok
- az adott seregben részt vevő házak (lehet több is)
- sereg mérete

```xml
<defender>
  <king>Robb Stark</king>
  <commanders>
    <commander>Clement Piper</commander>
    <commander>Vance</commander>
  </commanders>
  <house>Tully</house>
  <size>4000</size>
</defender>
```

## Feladat

Írj LINQ lekérdezéseket a következő kérdések megválaszolására, feladatok megoldására!

Q1. Hány ház vett részt a csatákban?

Q2. Listázzuk az „ambush” típusú csatákat!

Q3. Hány olyan csata volt, ahol a védekező sereg győzött, és volt híres fogoly?

Q4. Hány csatát nyert a Stark ház?

Q5. Mely csatákban vett részt több, mint 2 ház?

Q6. Melyik volt a 3 leggyakrabban előforduló régió?

Q7. Melyik volt a leggyakoribb régió?

Q8. A 3 leggyakrabban előforduló régióban mely csatákban vett részt több, mint 2 ház? (Q5 join Q6)

Q9. Listázzuk a házakat nyert csaták szerinti csökkenő sorrendben!

Q10. Mely csatában vett részt a legnagyobb ismert sereg?

Q11. Listázzuk a 3 leggyakrabban támadó parancsnokot!