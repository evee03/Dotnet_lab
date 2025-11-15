# Laboratorium 6 - WALIDACJA DANYCH UŻYTKOWNIKA PO STRONIE APLIKACJI

Projekt zawiera implementację walidacji danych w aplikacji ASP.NET Core MVC z wykorzystaniem atrybutów oraz obsługę relacji pomiędzy tabelami w Entity Framework Core. Aplikacja rozszerza funkcjonalność katalogu filmów o walidację formularzy oraz system gatunków filmowych.

## Zawartość

- **Zadanie 6.1** - Dodawanie ograniczeń do pól modelu
- **Zadanie 6.2** - Własne opisy błędów
- **Zadanie 6.3** - Walidacja po stronie klienta
- **Zadanie 6.4** - Dodawanie kolejnej tabeli oraz relacji pomiędzy tabelami
- **Zadanie 6.5** - Modyfikacja migracji Genre
- **Zadanie 6.6** - Wyświetlanie gatunku filmowego na liście
- **Zadanie 6.7** - Obsługa dodawania nowego filmu i jego gatunku
- **Zadanie 6.8** - Wykorzystanie elementu `<datalist>` do tworzenia podpowiedzi
- **Zadanie 6.9** - Modyfikacja formularza edycji filmu

## Opis zadań

### Zadanie 6.1: Dodawanie ograniczeń do pól modelu

Implementacja walidacji danych za pomocą atrybutów w modelu Movie.

**Modyfikacja klasy Movie (Models/Movie.cs):**

```csharp
using System.ComponentModel.DataAnnotations;

public class Movie
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; }

    [UIHint("LongText")]
    [Required]
    public string Description { get; set; }

    [UIHint("Stars")]
    [Range(1, 5)]
    public int Rating { get; set; }

    public string? TrailerLink { get; set; }

    public Genre Genre { get; set; }
}
```

**Kluczowe atrybuty walidacji:**

- `[Required]` - pole wymagane
- `[MaxLength(n)]` - maksymalna długość tekstu
- `[Range(min, max)]` - zakres wartości liczbowych
- `[Key]` - klucz główny tabeli
- `?` - nullable type (typ dopuszczający null)

**Wykonanie migracji:**

```bash
dotnet ef migrations add Limits
dotnet ef database update
```

### Zadanie 6.2: Własne opisy błędów

Personalizacja komunikatów walidacji dla lepszego doświadczenia użytkownika.

**Przykład użycia ErrorMessage:**

```csharp
[Required(ErrorMessage = "Tytuł jest wymagany")]
[MaxLength(50, ErrorMessage = "Tytuł może mieć maksymalnie 50 znaków")]
public string Title { get; set; }

[Required(ErrorMessage = "Opis jest wymagany")]
public string Description { get; set; }

[Range(1, 5, ErrorMessage = "Ocena filmu musi być liczbą pomiędzy 1 a 5")]
public int Rating { get; set; }
```

**Zalety własnych komunikatów:**

- Komunikaty w języku użytkownika (np. polskim)
- Lepsze zrozumienie wymagań
- Spójny styl komunikacji z użytkownikiem

### Zadanie 6.3: Walidacja po stronie klienta

Implementacja walidacji JavaScript dla natychmiastowej weryfikacji danych bez przesyłania formularza.

**Modyfikacja Views/Shared/\_Layout.cshtml:**

Dodaj na końcu, po wszystkich elementach `<script>`:

```razor
<partial name="_ValidationScriptsPartial" />
```

**Mechanizm walidacji:**

- Wykorzystuje jQuery Validation
- Opiera się na bibliotece Unobtrusive Validation
- Walidacja odbywa się przed wysłaniem formularza
- Używa atrybutów `data-val-*` w HTML

**Zalety walidacji po stronie klienta:**

- Natychmiastowa informacja zwrotna
- Brak niepotrzebnego ruchu sieciowego
- Lepsza responsywność aplikacji
- Walidacja po stronie serwera nadal pozostaje jako zabezpieczenie

### Zadanie 6.4: Dodawanie kolejnej tabeli oraz relacji pomiędzy tabelami

Rozszerzenie systemu o tabelę gatunków filmowych z relacją jeden-do-wielu.

**Utworzenie modelu Genre (Models/Genre.cs):**

```csharp
using System.ComponentModel.DataAnnotations;

public class Genre
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
}
```

**Modyfikacja kontekstu bazy danych (Data/MoviesDbContext.cs):**

```csharp
public class MoviesDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public MoviesDbContext(DbContextOptions options) : base(options)
    {
    }
}
```

**Dodanie relacji w klasie Movie:**

```csharp
public class Movie
{
    // ... inne właściwości

    public Genre Genre { get; set; }
}
```

**Utworzenie migracji:**

```bash
dotnet ef migrations add Genre
```

**⚠️ Uwaga:** Nie wykonuj jeszcze `dotnet ef database update` - pojawi się błąd integralności klucza obcego dla istniejących rekordów.

### Zadanie 6.5: Modyfikacja migracji Genre

Ręczna modyfikacja migracji w celu obsługi istniejących danych.

**Modyfikacja pliku Migrations/[data]\_Genre.cs:**

W metodzie `Up()` zmodyfikuj domyślną wartość kolumny `GenreId`:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "Genres",
        // ... definicja kolumn
    );

    migrationBuilder.AddColumn<int>(
        name: "GenreId",
        table: "Movies",
        nullable: false,
        defaultValue: 1  // Dodaj tę linię
    );

    // Dodaj domyślny gatunek
    migrationBuilder.InsertData(
        "Genres",
        new string[] { "Id", "Name" },
        new object[] { "1", "unknown" }
    );
}
```

**Wykonanie aktualizacji bazy:**

```bash
dotnet ef database update
```

**Kluczowe koncepcje:**

- Ręczna modyfikacja migracji dla złożonych scenariuszy
- `InsertData()` - dodawanie danych początkowych (seed data)
- `defaultValue` - wartość domyślna dla istniejących rekordów
- Zachowanie integralności danych w systemach produkcyjnych

### Zadanie 6.6: Wyświetlanie gatunku filmowego na liście

Implementacja wyświetlania gatunku w widoku Index.

**Modyfikacja Views/Home/Index.cshtml:**

Dodaj kolumnę w nagłówku tabeli:

```razor
<thead>
    <tr>
        <th>@Html.DisplayNameFor(model => model.Title)</th>
        <th>@Html.DisplayNameFor(model => model.Rating)</th>
        <th>@Html.DisplayNameFor(model => model.Genre)</th>
        <th>Actions</th>
    </tr>
</thead>
```

Dodaj komórkę w ciele tabeli:

```razor
@foreach (var item in Model)
{
    <tr>
        <td>@Html.DisplayFor(modelItem => item.Title)</td>
        <td>@Html.DisplayFor(modelItem => item.Rating)</td>
        <td>@Html.DisplayFor(modelItem => item.Genre.Name)</td>
        <td>
            <!-- linki akcji -->
        </td>
    </tr>
}
```

**Modyfikacja HomeController.cs (metoda Index):**

```csharp
public async Task<IActionResult> Index()
{
    var movies = await _context.Movies
        .Include(x => x.Genre)
        .ToListAsync();

    return View(movies);
}
```

**Kluczowe elementy:**

- `.Include()` - eager loading relacji (zapobiega lazy loading)
- `Genre.Name` - dostęp do właściwości powiązanej encji
- Bez `.Include()` wartość `Genre` będzie null

### Zadanie 6.7: Obsługa dodawania nowego filmu i jego gatunku

Implementacja modelu DTO (Data Transfer Object) do obsługi formularza z gatunkiem tekstowym.

**Utworzenie klasy MovieDto (Models/MovieDto.cs):**

```csharp
using System.ComponentModel.DataAnnotations;

public class MovieDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tytuł jest wymagany")]
    [MaxLength(50)]
    public string Title { get; set; }

    [UIHint("LongText")]
    [Required(ErrorMessage = "Opis jest wymagany")]
    public string Description { get; set; }

    [UIHint("Stars")]
    [Range(1, 5, ErrorMessage = "Ocena filmu musi być liczbą pomiędzy 1 a 5")]
    public int Rating { get; set; }

    public string? TrailerLink { get; set; }

    public string Genre { get; set; }

    public List<string>? AllGenres { get; set; }
}
```

**Modyfikacja HomeController.cs (metoda Create POST):**

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(
    [Bind("Id,Title,Description,Rating,TrailerLink,Genre")] MovieDto movie)
{
    if (ModelState.IsValid)
    {
        // Wyszukaj istniejący gatunek lub stwórz nowy
        var genre = _context.Genres.FirstOrDefault(x => x.Name == movie.Genre);
        if (genre == null)
        {
            genre = new Genre { Id = 0, Name = movie.Genre };
        }

        // Mapowanie MovieDto na Movie
        Movie m = new Movie
        {
            Id = 0,
            Title = movie.Title,
            Description = movie.Description,
            Rating = movie.Rating,
            TrailerLink = movie.TrailerLink,
            Genre = genre
        };

        _context.Add(m);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    return View(movie);
}
```

**Modyfikacja Views/Home/Create.cshtml:**

Zmień dyrektywę `@model`:

```razor
@model MovieDto
```

Dodaj pole dla gatunku:

```razor
<div class="form-group">
    <label asp-for="Genre" class="control-label"></label>
    <input asp-for="Genre" class="form-control" />
    <span asp-validation-for="Genre" class="text-danger"></span>
</div>
```

**Wzorzec DTO:**

- Separacja modelu bazodanowego od modelu widoku
- Większa elastyczność w definiowaniu formularzy
- Możliwość walidacji specyficznej dla konkretnego przypadku użycia
- W produkcji: użycie AutoMapper do automatycznego mapowania

### Zadanie 6.8: Wykorzystanie elementu `<datalist>` do tworzenia podpowiedzi

Implementacja autosugestii dla pola gatunku z wykorzystaniem HTML5.

**Modyfikacja Views/Home/Create.cshtml:**

```razor
<div class="form-group">
    <label asp-for="Genre" class="control-label"></label>
    <input asp-for="Genre" class="form-control" list="genres" />
    <datalist id="genres">
        @foreach (var item in Model.AllGenres)
        {
            @Html.Raw($"<option value=\"{item}\">")
        }
    </datalist>
    <span asp-validation-for="Genre" class="text-danger"></span>
</div>
```

**Modyfikacja HomeController.cs (metoda Create GET):**

```csharp
public IActionResult Create()
{
    var m = new MovieDto
    {
        AllGenres = _context.Genres.Select(x => x.Name).ToList()
    };
    return View(m);
}
```

**Element `<datalist>`:**

- Standard HTML5
- Autouzupełnianie z listą sugestii
- Użytkownik może wybrać z listy lub wpisać własną wartość
- Nie wymaga JavaScript
- Atrybut `list` w `<input>` łączy z `id` elementu `<datalist>`

**Zalety:**

- Przyjazny interfejs użytkownika
- Możliwość wyboru istniejącego gatunku
- Możliwość dodania nowego gatunku
- Natywna funkcjonalność przeglądarki

### Zadanie 6.9: Modyfikacja formularza edycji filmu

Rozszerzenie funkcjonalności edycji o obsługę gatunków.

**Modyfikacja Views/Home/Edit.cshtml:**

Zmień dyrektywę `@model`:

```razor
@model MovieDto
```

Dodaj pole dla gatunku (analogicznie do Create.cshtml):

```razor
<div class="form-group">
    <label asp-for="Genre" class="control-label"></label>
    <input asp-for="Genre" class="form-control" list="genres" />
    <datalist id="genres">
        @foreach (var item in Model.AllGenres)
        {
            @Html.Raw($"<option value=\"{item}\">")
        }
    </datalist>
    <span asp-validation-for="Genre" class="text-danger"></span>
</div>
```

**Modyfikacja HomeController.cs (metoda Edit GET):**

```csharp
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var movie = _context.Movies
        .Include(x => x.Genre)
        .FirstOrDefault(x => x.Id == id);

    if (movie == null)
    {
        return NotFound();
    }

    var movieDto = new MovieDto
    {
        Id = movie.Id,
        Title = movie.Title,
        Description = movie.Description,
        Rating = movie.Rating,
        TrailerLink = movie.TrailerLink,
        Genre = movie.Genre.Name,
        AllGenres = _context.Genres.Select(x => x.Name).ToList()
    };

    return View(movieDto);
}
```

**Modyfikacja HomeController.cs (metoda Edit POST):**

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id,
    [Bind("Id,Title,Description,Rating,TrailerLink,Genre")] MovieDto movie)
{
    if (id != movie.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            var genre = _context.Genres.FirstOrDefault(x => x.Name == movie.Genre);
            if (genre == null)
            {
                genre = new Genre { Id = 0, Name = movie.Genre };
            }

            var m = await _context.Movies.FindAsync(id);
            m.Title = movie.Title;
            m.Description = movie.Description;
            m.Rating = movie.Rating;
            m.TrailerLink = movie.TrailerLink;
            m.Genre = genre;

            _context.Update(m);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(movie.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }

    movie.AllGenres = _context.Genres.Select(x => x.Name).ToList();
    return View(movie);
}
```

**⚠️ Ważne:**

- Użyj `.Include(x => x.Genre)` zamiast `.Find()` do pobrania filmu z gatunkiem
- Mapuj dane z `Movie` na `MovieDto` i odwrotnie
- Wypełnij `AllGenres` przed zwróceniem widoku w przypadku błędu walidacji

## Uruchomienie

### Wymagania

- .NET 8.0 SDK lub nowszy
- Visual Studio 2022 / VS Code + C# Extension / Rider
- SQLite
- Ukończone Laboratorium 5

### Kompilacja i uruchomienie

```bash
# Przywróć zależności
dotnet restore

# Wykonaj migracje
dotnet ef migrations add Limits
dotnet ef migrations add Genre
dotnet ef database update

# Uruchom aplikację
dotnet build
dotnet run
```

Aplikacja będzie dostępna pod adresem: `http://localhost:5000`

---

**Projekt wykonany w ramach zajęć laboratoryjnych z aplikacji internetowych ASP.NET Core.**

**"Dane bez walidacji to jak drzwi bez zamka" - jakiś programista** 🔒
