# Laboratorium 3 - Aplikacja internetowa w architekturze MVC

Projekt zawiera implementację aplikacji ASP.NET Core wykorzystującej architekturę Model-View-Controller (MVC) z silnikiem widoków Razor, Dependency Injection oraz obsługą formularzy.

## Zawartość

- **Zadanie 3.1** - Generowanie nowego projektu MVC
- **Zadanie 3.2** - Silnik widoków Razor
- **Zadanie 3.3** - Przekazywanie danych między kontrolerem a widokiem
- **Zadanie 3.4** - Silnie typowane modele danych
- **Zadanie 3.5** - Repozytorium danych (PhoneBookService)
- **Zadanie 3.6** - Wstrzykiwanie zależności i prezentacja danych
- **Zadanie 3.7** - Dodawanie nowych elementów
- **Zadanie 3.8** - Usuwanie kontaktów
- **Zadanie 3.9** - Zabezpieczenie usuwania

## Opis zadań

### Zadanie 3.1: Generowanie projektu MVC

Utworzenie nowego projektu ASP.NET Core z szablonu **"ASP.NET Core Web App (Model-View-Controller)"**.

**Kluczowe elementy projektu:**

- Controllers/ - kontrolery aplikacji
- Views/ - widoki Razor (.cshtml)
- Models/ - modele danych
- wwwroot/ - pliki statyczne (CSS, JS, obrazy)

### Zadanie 3.2: Silnik widoków Razor

Wykorzystanie składni Razor do dynamicznego generowania HTML:

- Znak `@` do osadzania kodu C# w HTML
- Formatowanie dat: `@DateTime.Now.ToString("yyyy")`
- Modyfikacja layoutu aplikacji (`_Layout.cshtml`)
- Automatyczne wyświetlanie aktualnego roku w stopce

**Przykład:**

```razor
@DateTime.Now.ToString("D")  <!-- Format daty długiej -->
```

### Zadanie 3.3: Przekazywanie danych przez ViewData

Mechanizm `ViewData` do przekazywania danych z kontrolera do widoku:

```csharp
// Kontroler
ViewData["random"] = r.NextDouble();

// Widok
@ViewData["random"]
```

**Rozszerzenie:** Warunkowe formatowanie - czerwone tło dla wartości > 0.5

### Zadanie 3.4: Silnie typowane modele

Utworzenie klasy `Contact` z właściwościami:

- Id (int)
- Name, Surname, Email, City, PhoneNumber (string)
- Atrybuty `[DisplayName]` dla przyjaznych nazw

```csharp
public class Contact
{
    [DisplayName("Identyfikator")]
    public int Id { get; set; }

    [DisplayName("Imię")]
    public string Name { get; set; }

    // ... pozostałe właściwości
}
```

### Zadanie 3.5: Repozytorium danych

Implementacja klasy `PhoneBookService`:

- Przechowywanie kontaktów w liście
- Metoda `GetContacts()` zwracająca `IEnumerable<Contact>`
- Wzorzec Singleton przez Dependency Injection

**Rejestracja serwisu:**

```csharp
builder.Services.AddSingleton<PhoneBookService>();
```

### Zadanie 3.6: Dependency Injection

Wstrzykiwanie `PhoneBookService` do kontrolera:

```csharp
private readonly PhoneBookService _phoneBook;

public HomeController(ILogger<HomeController> logger,
                      PhoneBookService phoneBook)
{
    _logger = logger;
    _phoneBook = phoneBook;
}
```

Przekazywanie danych do silnie typowanego widoku:

```csharp
public IActionResult Index2()
{
    return View(_phoneBook.GetContacts());
}
```

### Zadanie 3.7: Dodawanie kontaktów

Generowanie widoku z formularzem (scaffolding):

- Widok `Create.cshtml` z szablonem "Create"
- Dwie akcje kontrolera: GET (wyświetlenie) i POST (przetworzenie)
- Model Binding - automatyczne mapowanie pól formularza

```csharp
public IActionResult Create()
{
    return View();
}

[HttpPost]
public IActionResult Create(Contact contact)
{
    if (ModelState.IsValid)
    {
        _phoneBook.Add(contact);
        return RedirectToAction("Index2");
    }
    return View();
}
```

### Zadanie 3.8: Usuwanie kontaktów

Implementacja funkcji usuwania:

- Generowanie linków z `ActionLink()`
- Przekazywanie ID przez parametry URL
- Akcja `Delete()` w kontrolerze

```csharp
@Html.ActionLink("Delete", "Delete", new { id = item.Id })

public IActionResult Delete(int id)
{
    _phoneBook.Remove(id);
    return RedirectToAction("Index2");
}
```

### Zadanie 3.9: Zabezpieczenie operacji

Obsługa błędów przy nieistniejących ID:

- Zwracanie kodu HTTP 404 (NotFound)
- Rozdzielenie odpowiedzialności: serwis vs kontroler
- Walidacja danych wejściowych

## Uruchomienie

### Wymagania

- .NET 6.0 SDK lub nowszy
- Visual Studio 2022 / VS Code + C# Extension / Rider
- Przeglądarką internetowa

### Kompilacja i uruchomienie

```bash
dotnet build
dotnet run
```

Aplikacja będzie dostępna pod adresem: `https://localhost:5001` lub `http://localhost:5000`

### W Visual Studio

Naciśnij **F5** (Start Debugging) lub **Ctrl+F5** (Start Without Debugging)

## Routing i nawigacja

### Dostępne endpointy

- `/` lub `/Home/Index` - strona główna
- `/Home/Index2` - lista kontaktów
- `/Home/Create` - formularz dodawania kontaktu
- `/Home/Delete/{id}` - usuwanie kontaktu
- `/Home/Privacy` - polityka prywatności

### Domyślny routing

```
{controller=Home}/{action=Index}/{id?}
```

## 🔧 Wykorzystane technologie i koncepcje

### ASP.NET Core MVC

- **Kontrolery** - obsługa żądań HTTP
- **Akcje kontrolera** - metody odpowiadające na żądania
- **Routing** - mapowanie URL na akcje
- **Model Binding** - automatyczne mapowanie danych

### Razor

- Składnia `@` do osadzania C#
- Dyrektywy `@model`, `@using`
- Tag Helpers i HTML Helpers
- Layout i częściowe widoki (Partial Views)

### Dependency Injection

- Wbudowany kontener IoC
- Cykle życia: Singleton, Scoped, Transient
- Wstrzykiwanie przez konstruktor

### Model-View-Controller

- Rozdzielenie warstw aplikacji
- ViewData i ViewBag
- Silnie typowane widoki
- RedirectToAction

## Dodatkowe informacje

Projekt wykonany w ramach zajęć laboratoryjnych z aplikacji internetowych ASP.NET Core MVC.

---

**Zawsze programuj jak gdyby osoba zajmująca się twoim kodem w przyszłości była agresywnym psychopatą, który wie gdzie mieszkasz." -- Martin Golding 🎓**
