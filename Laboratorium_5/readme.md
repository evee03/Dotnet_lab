# Laboratorium 5 - TWORZENIE APLIKACJI INTERNETOWEJ KORZYSTAJĄCEJ Z BAZY DANYCH ORAZ MECHANIZMU ORM

Projekt zawiera implementację aplikacji ASP.NET Core MVC wykorzystującej Entity Framework Core jako ORM do obsługi bazy danych SQLite. Aplikacja umożliwia zarządzanie katalogiem filmów z wykorzystaniem operacji CRUD.

## Zawartość

- **Zadanie 5.1** - Generowanie aplikacji MVC i dodawanie zależności
- **Zadanie 5.2** - Tworzenie modelu danych
- **Zadanie 5.3** - Sekrety i połączenie do bazy danych
- **Zadanie 5.4** - Generowanie kontrolera CRUD
- **Zadanie 5.5** - Zmiana formy wprowadzania danych (szablony edytorów)
- **Zadanie 5.6** - Zmiana formy wyświetlania danych (szablony wyświetlania)
- **Zadanie 5.7** - Dodawanie hiperłączy
- **Zadanie 5.8** - Wyświetlanie zwiastuna jako iframe

## Opis zadań

### Zadanie 5.1: Generowanie aplikacji MVC i dodawanie zależności

Utworzenie nowego projektu ASP.NET Core z szablonu **"ASP.NET Core Web App (Model-View-Controller)"**.

**Kluczowe biblioteki:**

- `Microsoft.EntityFrameworkCore.Sqlite` - provider bazy danych SQLite dla EF Core

### Zadanie 5.2: Tworzenie modelu danych

Implementacja modelu danych reprezentującego film w bazie danych.

**Klasa Movie (Models/Movie.cs):**

```csharp
using System.ComponentModel.DataAnnotations;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
    public string TrailerLink { get; set; }
}
```

**Uwaga:** Jeżeli właściwość identyfikatora nie nazywa się `Id`, należy dodać atrybut `[Key]`.

**Kontekst bazy danych (Data/MoviesDbContext.cs):**

```csharp
using Microsoft.EntityFrameworkCore;

public class MoviesDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    public MoviesDbContext(DbContextOptions options) : base(options)
    {
    }
}
```

**Kluczowe elementy:**

- `DbContext` - główna klasa do interakcji z bazą danych w EF Core
- `DbSet<T>` - reprezentuje kolekcję encji w bazie (tabelę)
- **Ważne:** `DbSet<T>` musi być właściwością, nie polem
- Klasa `Movie` musi być publiczna

### Zadanie 5.3: Sekrety i połączenie do bazy danych

Konfiguracja bezpiecznego przechowywania danych połączenia z bazą danych.

**Zawartość secrets.json (Visual Studio):**

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=movies.db"
  }
}
```

**Rejestracja kontekstu w Program.cs:**

Dodaj przed `var app = builder.Build();`:

```csharp
builder.Services.AddDbContext<MoviesDbContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("Default"))
);
```

**Connection String:**

- Mechanizm definiowania parametrów połączenia z bazą danych
- Dla SQLite: tylko nazwa pliku bazy
- Dla innych baz: serwer, port, login, hasło, itp.

**Przykład złożonego connection string:**

```
Server=example.com;Port=3306;Database=iai;Uid=user;Pwd=pass;
```

### Zadanie 5.4: Generowanie kontrolera CRUD

Automatyczne wygenerowanie kontrolera z pełną funkcjonalnością CRUD.

**Linia komend:**

Instalacja narzędzi:

```bash
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 8.0.*
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.*
```

Generowanie kontrolera:

```bash
dotnet aspnet-codegenerator controller -name HomeController -m Movie -dc MoviesDbContext -udl -sqlite -outDir Controllers
```

**Tworzenie i migracja bazy danych:**

```bash
dotnet ef migrations add Initial

dotnet ef database update
```

**Migracje:**

- Mechanizm śledzenia zmian w schemacie bazy danych
- Folder `Migrations/` zawiera historię zmian
- Umożliwia wersjonowanie struktury bazy danych

### Zadanie 5.5: Zmiana formy wprowadzania danych

Implementacja niestandardowego szablonu edycji dla pola tekstowego.

**Modyfikacja widoków Create.cshtml i Edit.cshtml:**

Zmień linię z `<input>` na:

```razor
@Html.EditorFor(m => m.Description)
```

**Utworzenie szablonu edytora (Views/Shared/EditorTemplates/LongText.cshtml):**

```razor
@model string
@Html.TextArea("", Model, new { cols = 40, rows = 10, @class="form-control" })
```

**Dodanie atrybutu do modelu:**

```csharp
using System.ComponentModel.DataAnnotations;

public class Movie
{
    // ... inne właściwości

    [UIHint("LongText")]
    public string Description { get; set; }
}
```

**Kluczowe elementy:**

- `EditorFor()` - automatyczny wybór szablonu edycji
- `EditorTemplates/` - folder z niestandardowymi szablonami edytorów
- `[UIHint]` - wskazanie konkretnego szablonu
- `@class` - użycie słowa kluczowego C# jako nazwy zmiennej

### Zadanie 5.6: Zmiana formy wyświetlania danych

Implementacja niestandardowego szablonu wyświetlania oceny jako gwiazdek.

**Utworzenie szablonu (Views/Shared/DisplayTemplates/Stars.cshtml):**

Podstawowa wersja:

```razor
@model int
@for (int i = 0; i < Model; i++)
{
    @Html.Raw("&#x2606;")
}
```

**Rozszerzona wersja (wypełnione gwiazdki + maksymalnie 5):**

```razor
@model int

<span title="Ocena: @Model/5">
    @for (int i = 0; i < Model; i++)
    {
        @Html.Raw("&#x2605;")
    }
    @for (int i = Model; i < 5; i++)
    {
        @Html.Raw("&#x2606;")
    }
</span>
```

**Dodanie atrybutu do modelu:**

```csharp
[UIHint("Stars")]
public int Rating { get; set; }
```

**Encje HTML:**

- `&#x2606;` - pusta gwiazdka (☆)
- `&#x2605;` - wypełniona gwiazdka (★)
- `Html.Raw()` - renderowanie surowego HTML bez escapowania

### Zadanie 5.7: Dodawanie hiperłączy

Zamiana tekstowego URL na klikalny link do zwiastuna.

**Modyfikacja Views/Home/Index.cshtml:**

Znajdź linię wyświetlającą `TrailerLink` i zamień na:

```razor
<td>
    <a href="@item.TrailerLink" target="_blank">Trailer</a>
</td>
```

lub z użyciem Tag Helpers:

```razor
<td>
    <a asp-action="Details" asp-route-id="@item.Id">Details</a> |
    <a href="@item.TrailerLink" target="_blank" rel="noopener">Trailer</a>
</td>
```

**Elementy:**

- `target="_blank"` - otwórz link w nowej karcie
- `rel="noopener"` - bezpieczeństwo przy otwieraniu w nowej karcie

### Zadanie 5.8: Wyświetlanie zwiastuna jako iframe

Osadzenie odtwarzacza YouTube bezpośrednio na stronie szczegółów filmu.

**Modyfikacja Views/Home/Details.cshtml:**

Znajdź sekcję wyświetlającą `TrailerLink` i zamień na:

```razor
<dt class="col-sm-2">
    @Html.DisplayNameFor(model => model.TrailerLink)
</dt>
<dd class="col-sm-10">
    @{
        var embedUrl = Model.TrailerLink.Replace("watch?v=", "embed/");
    }
    <iframe width="640" height="360"
            src="@embedUrl"
            frameborder="0"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
            allowfullscreen>
    </iframe>
</dd>
```

**Transformacja URL:**

- YouTube standardowy: `https://www.youtube.com/watch?v=dQw4w9WgXcQ`
- YouTube embed: `https://www.youtube.com/embed/dQw4w9WgXcQ`
- Użycie `Replace()` do konwersji

**Atrybuty iframe:**

- `width`, `height` - wymiary odtwarzacza
- `frameborder="0"` - brak ramki
- `allow` - uprawnienia (autoplay, fullscreen, itp.)
- `allowfullscreen` - możliwość pełnego ekranu

## Uruchomienie

### Wymagania

- .NET 8.0 SDK lub nowszy
- Visual Studio 2022 / VS Code + C# Extension / Rider
- SQLite (wbudowane w system lub jako biblioteka)

### Kompilacja i uruchomienie

```bash

dotnet restore

dotnet ef migrations add Initial

dotnet ef database update

dotnet build

dotnet run
```

Aplikacja będzie dostępna pod adresem: `http://localhost:5000`

## Wykorzystane technologie i koncepcje

### Entity Framework Core

- **ORM (Object-Relational Mapping)** - mapowanie obiektów na tabele bazodanowe
- **DbContext** - główny punkt dostępu do bazy danych
- **DbSet<T>** - kolekcja encji reprezentująca tabelę
- **Migracje** - wersjonowanie schematu bazy danych
- **Code First** - tworzenie bazy na podstawie modeli C#

### Architektura MVC

- **Model** - klasy reprezentujące dane (Movie)
- **View** - widoki Razor (pliki .cshtml)
- **Controller** - logika biznesowa i obsługa żądań

### Scaffolding (Generatory kodu)

- Automatyczne generowanie kontrolerów i widoków CRUD
- Szablony oparte o Entity Framework
- Znaczne przyspieszenie tworzenia aplikacji

### Szablony wyświetlania i edycji

**EditorTemplates:**

- Niestandardowe kontrolki edycji
- Lokalizacja: `Views/Shared/EditorTemplates/`
- Wybór przez `[UIHint]` lub automatycznie według typu

**DisplayTemplates:**

- Niestandardowe sposoby wyświetlania danych
- Lokalizacja: `Views/Shared/DisplayTemplates/`
- Separacja logiki prezentacji od widoków

### User Secrets

- Bezpieczne przechowywanie wrażliwych danych
- Lokalizacja poza repozytorium kodu
- Ochrona mechanizmami systemu operacyjnego
- Przydatne dla: connection strings, klucze API, hasła

### SQLite

- Lekka, plikowa baza danych
- Idealna do prototypowania i małych aplikacji
- Brak wymogu osobnego serwera bazodanowego
- Łatwa migracja na inne bazy (SQL Server, PostgreSQL)

## Operacje CRUD

| Operacja   | Akcja kontrolera       | Opis                            |
| ---------- | ---------------------- | ------------------------------- |
| **C**reate | `Create()`             | Tworzenie nowego filmu          |
| **R**ead   | `Index()`, `Details()` | Wyświetlanie listy i szczegółów |
| **U**pdate | `Edit()`               | Edycja istniejącego filmu       |
| **D**elete | `Delete()`             | Usuwanie filmu                  |

Każda akcja posiada wariant GET (wyświetlenie) i POST (przetworzenie danych).

## Komendy Entity Framework

```bash
dotnet ef migrations add NazwaMigracji

dotnet ef database update

dotnet ef database update NazwaPierwszejMigracji

dotnet ef migrations remove

dotnet ef migrations script

dotnet ef dbcontext info
```

## Różnice między Code First a Database First

| Aspekt        | Code First                 | Database First         |
| ------------- | -------------------------- | ---------------------- |
| Punkt wyjścia | Klasy C#                   | Istniejąca baza danych |
| Migracje      | Automatyczne tworzenie     | Reverse engineering    |
| Kontrola      | Pełna kontrola nad modelem | Model oparty na bazie  |
| Zastosowanie  | Nowe projekty              | Istniejące systemy     |

W tym laboratorium używamy podejścia **Code First**.

## Zaawansowane funkcje (opcjonalnie)

### Walidacja danych

```csharp
using System.ComponentModel.DataAnnotations;

public class Movie
{
    [Required(ErrorMessage = "Tytuł jest wymagany")]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }

    [Range(1, 5, ErrorMessage = "Ocena musi być od 1 do 5")]
    public int Rating { get; set; }

    [Url(ErrorMessage = "Nieprawidłowy adres URL")]
    public string TrailerLink { get; set; }
}
```

### Relacje między tabelami

```csharp
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }

    // Relacja wiele-do-jednego
    public int DirectorId { get; set; }
    public Director Director { get; set; }

    // Relacja wiele-do-wielu
    public ICollection<Actor> Actors { get; set; }
}
```

## Pytania kontrolne

1. **Czym są zależności (dependencies) w dużych systemach informatycznych?**

   - Zewnętrzne biblioteki i pakiety wykorzystywane przez aplikację
   - Zarządzane przez menedżery pakietów (NuGet dla .NET)
   - Umożliwiają reużywalność kodu i przyspieszenie rozwoju
   - Wymagają śledzenia wersji i aktualizacji

2. **Do czego służą biblioteki typu ORM?**

   - Object-Relational Mapping - mapowanie obiektów na tabele bazodanowe
   - Abstrakcja nad niskopoziomowym SQL
   - Automatyczne generowanie zapytań
   - Ułatwienie pracy z różnymi bazami danych
   - Przykłady: Entity Framework, Hibernate, Django ORM

3. **W jaki sposób prezentowane są dane słownikowe w HTML?**
   - Listy rozwijane: `<select>` z `<option>`
   - Radio buttons: `<input type="radio">`
   - Checkboxy: `<input type="checkbox">`
   - W ASP.NET: Tag Helpers, DisplayTemplates, EditorTemplates

## Najczęstsze problemy i rozwiązania

### Problem: Migracja się nie tworzy

```bash
dotnet tool install --global dotnet-ef

dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Problem: Baza danych nie aktualizuje się

```bash
dotnet ef database drop
dotnet ef database update
```

### Problem: "No DbContext was found"

- Upewnij się, że kontekst jest zarejestrowany w `Program.cs`
- Sprawdź czy nazwa kontekstu w komendzie jest poprawna

---

**Projekt wykonany w ramach zajęć laboratoryjnych z aplikacji internetowych ASP.NET Core.**

**"Dane bez struktury to jak książka bez spisu treści" - nieznany** 📊
