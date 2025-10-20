# Laboratorium 2 - Programowanie obiektowe i LINQ

Laboratorium zawiera implementację zaawansowanych koncepcji C# obejmujących programowanie obiektowe, LINQ, asynchroniczność, formatowanie kulturowe oraz testy jednostkowe.

## Zawartość

- **Zadanie 2.1** - Klasa modelu danych i metoda fabrykująca
- **Zadanie 2.2** - Przeciążanie konwersji do łańcucha tekstowego
- **Zadanie 2.3** - Wyświetlanie tylko wybranych elementów (LINQ)
- **Zadanie 2.4** - Metody asynchroniczne
- **Zadanie 2.5** - Właściwości obliczane na bieżąco
- **Zadanie 2.6** - Formatowanie zależne od kultury
- **Zadanie 2.7** - Testy jednostkowe
- **Zadanie 2.8** - Udoskonalenie testów
- **Zadanie 2.9** - Testy losowości

## Opis zadań

### Zadanie 2.1: Klasa modelu danych i metoda fabrykująca

Utworzenie klasy `Fruit` z trzema właściwościami:

- `Name` (string) - nazwa owocu
- `IsSweet` (bool) - czy owoc jest słodki
- `Price` (double) - cena w złotówkach

Implementacja statycznej metody fabrykującej `Create()`, która:

- Losuje nazwę z predefiniowanej listy owoców
- Generuje losowe wartości dla `IsSweet` i `Price`
- Zwraca gotowy obiekt `Fruit`

**Przykład użycia:**

```csharp
List<Fruit> fruits = new List<Fruit>();
for (int i = 0; i < 15; i++)
{
    fruits.Add(Fruit.Create());
}
```

### Zadanie 2.2: Przeciążanie ToString()

Implementacja metody `ToString()` z modyfikatorem `override`:

- Formatuje dane w czytelny sposób
- Wykorzystuje format walutowy "C2" dla ceny
- Wyświetla wszystkie właściwości owocu

**Przykładowy wynik:**

```
Fruit: Name=Apple, IsSweet=True, Price=7,85 zł
```

### Zadanie 2.3: LINQ - filtrowanie i sortowanie

Wykorzystanie LINQ do zaawansowanej manipulacji danymi:

- Filtrowanie tylko słodkich owoców (`Where`)
- Sortowanie malejąco według ceny (`OrderByDescending`)
- Leniwa ewaluacja z `IEnumerable` / `IQueryable`

**Przykład:**

```csharp
var sweetFruits = fruits
    .Where(x => x.IsSweet)
    .OrderByDescending(x => x.Price);
```

### Zadanie 2.4: Metody asynchroniczne

Implementacja klasy `UsdCourse` z asynchroniczną metodą:

- Pobieranie aktualnego kursu USD z API NBP
- Wykorzystanie `async/await` dla operacji nieblokujących
- Parsowanie odpowiedzi XML za pomocą `XDocument`
- Obsługa błędów HTTP

**Kluczowe elementy:**

- `Task<float>` jako "obietnica" zwrócenia wartości
- `HttpClient` do komunikacji z API
- LINQ to XML do parsowania odpowiedzi

### Zadanie 2.5: Właściwości obliczane

Dodanie właściwości `UsdPrice` jako wyrażenia strzałkowego:

```csharp
public double UsdPrice => Price / UsdCourse.Current;
```

- Brak settera (tylko getter)
- Dynamiczne obliczanie na podstawie aktualnego kursu
- Automatyczna aktualizacja przy zmianie kursu

### Zadanie 2.6: Formatowanie kulturowe (Locale)

Implementacja formatowania zależnego od kultury:

- Cena w PLN: format polski ("0,99 zł")
- Cena w USD: format amerykański ("$0.99")
- Wykorzystanie `CultureInfo("en-us")`

**Przykładowa metoda:**

```csharp
public static string FormatUsdPrice(double price)
{
    var usc = new CultureInfo("en-us");
    return price.ToString("C2", usc);
}
```

### Zadanie 2.7-2.9: Testy jednostkowe

Utworzenie projektu testowego z testami sprawdzającymi:

- **Test 2.7**: Format początkowy wyniku `ToString()`
- **Test 2.8**: Dokładny format całego ciągu zwracanego przez `ToString()`
- **Test 2.9**: Losowość metody `Create()` (różnorodność nazw)

**Przykładowy test:**

```csharp
[Fact]
public void Fruit_ProperFormat_ShouldStartWithFruit()
{
    var fruit = new Fruit { Name = "Apple" };
    var result = fruit.ToString();
    Assert.StartsWith("Fruit", result);
}
```

## Uruchomienie

### Wymagania

- .NET SDK 6.0 lub nowszy
- Połączenie z Internetem (do pobierania kursów walut)
- Visual Studio 2022 / VS Code / Rider (opcjonalnie)

### Kompilacja i uruchomienie projektu głównego

```bash
cd Laboratorium_2
dotnet build
dotnet run
```

### Uruchomienie testów

```bash
cd Laboratorium_2.Tests
dotnet test
```

Lub w Visual Studio: **Test Explorer** → **Run All Tests**

## API wykorzystane w projekcie

**NBP API** - Narodowy Bank Polski

- Endpoint: `https://api.nbp.pl/api/exchangerates/tables/a/?format=xml`
- Format odpowiedzi: XML
- Dane: Aktualne kursy walut (tabela A)

## 🔧 Wykorzystane technologie i koncepcje

### Programowanie obiektowe

- Właściwości automatyczne (auto-properties)
- Metody statyczne i fabrykujące
- Przeciążanie metod (`override`)
- Modyfikatory dostępu (`public`, `internal`)

### LINQ (Language Integrated Query)

- `Where()` - filtrowanie kolekcji
- `Select()` - projekcja danych
- `OrderBy()` / `OrderByDescending()` - sortowanie
- `FirstOrDefault()` - pobieranie pierwszego elementu
- Leniwa ewaluacja (`IEnumerable`, `IQueryable`)

### Asynchroniczność

- `async` / `await` - programowanie asynchroniczne
- `Task<T>` - reprezentacja operacji asynchronicznych
- `HttpClient` - komunikacja HTTP
- Operacje nieblokujące

### Formatowanie i kultura

- `CultureInfo` - informacje o kulturze/regionie
- Formatowanie walut (`ToString("C2")`)
- Lokalizacja danych

### Parsowanie XML

- `XDocument` - parsowanie i manipulacja XML
- LINQ to XML - zapytania do dokumentów XML

### Testy jednostkowe

- xUnit Framework
- Atrybuty `[Fact]`
- Asercje (`Assert.StartsWith`, `Assert.Equal`)
- Test-Driven Development (TDD)

## Dodatkowe informacje

Zadania wykonane w ramach zajęć laboratoryjnych z programowania i architektury w .NET.

---

**Każda poprawa tego kodu to tylko stały czynnik, a stały czynnik poprawy czegoś złego, nadal jest zły. --Profesor OS. 🎓**
