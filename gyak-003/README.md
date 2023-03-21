Készítsen el a Main függvény mellé egy DLL-t, amely a Validation.Classes névtéren belül megvalósítja az alábbi osztályokat:

- MaxLengthAttribute(int) - egy stringre konvertálható attribútum, melynek hosszára szeretnénk limitet megfogalmazni

- RangeAttribute(int, int) - numerikus értékű attribútum, melynek alsó és felső korlátjára szeretnénk limitet megfogalmazni

A tényleges validáláshoz:

- IValidation interfész
public bool validate(object instance, PropertyInfo prop)
metódussal, ami alkalmas lesz arra, hogy az adott instance-nek lekérdezzük a PropertyInfo által meghatározott tulajdonságát, és igaz/hamis visszatéréssel megmondjuk, hogy teljesül-e rá az adott feltétel

Ezt implementálja az alábbi 2 osztály:

- MaxLengthValidation(MaxLengthAttribute)
- RangeValidation(RangeAttribute)


A ValidationFactory osztály GetValidation(Attribute) metódusa visszaadja a megfelelő Validation osztályt, vagy
ha az Attribute argumentum sem MaxLengthAttribute, sem RangeAttribute akkor null-t ad vissza

Végül pedig a Validator osztálynak a
public bool Validate(object instance)
metódusa:
a.) végigmegy az instance összes tulajdonságán és lekéri mindegyik összes attribútumát
b.) a ValidationFactory osztály segítségével talán le tud kérni egy validation-t
c.) ha ez sikeres, akkor validál

