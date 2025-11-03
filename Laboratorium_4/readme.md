# Laboratorium 4 - Aplikacja internetowa w architekturze Razor Pages

Projekt zawiera implementację aplikacji ASP.NET Core wykorzystującej architekturę Razor Pages do obsługi przesyłania plików graficznych, ich prezentacji oraz manipulacji obrazami.

## Zawartość

- **Zadanie 4.1** - Generowanie projektu Razor Pages
- **Zadanie 4.2** - Dodawanie atrybutu do modelu
- **Zadanie 4.3** - Wysyłanie plików na serwer
- **Zadanie 4.4** - Dodawanie odnośnika do uploadu
- **Zadanie 4.5** - Widoki częściowe (Partial Views)
- **Zadanie 4.6** - Strona pojedynczego obrazka z routingiem
- **Zadanie 4.7** - Znaki wodne (Magick.NET)
- **Zadanie 4.8** - Walidacja rozmiaru pliku

## Opis zadań

### Zadanie 4.1: Generowanie projektu Razor Pages

Utworzenie nowego projektu ASP.NET Core z szablonu **"ASP.NET Core Web App"** (Razor Pages).

**Kluczowe elementy projektu:**

- Pages/ - strony Razor (.cshtml + .cshtml.cs)
- wwwroot/ - pliki statyczne (CSS, JS, obrazy)
- Brak folderów Controllers i Views (charakterystycznych dla MVC)

**Struktura Razor Pages:**

Każda strona składa się z dwóch plików:

- `.cshtml` - kod Razor (widok)
- `.cshtml.cs` - code-behind z modelem strony (PageModel)

**Metody w PageModel:**

- `OnGet()` - obsługa żądań GET
- `OnPost()` - obsługa żądań POST

### Zadanie 4.2: Dodawanie atrybutu do modelu

Implementacja dynamicznej listy plików graficznych z wykorzystaniem Dependency Injection.

**Dodanie właściwości do modelu:**

```csharp
public List<string> Images { get; set; }
```

**Wyświetlanie listy w widoku Razor:**

```razor
@foreach (var item in Model.Images)
{
    @item
}
```

**Wstrzykiwanie IWebHostEnvironment:**

```csharp
private string imagesDir;

public IndexModel(IWebHostEnvironment environment)
{
    imagesDir = Path.Combine(environment.WebRootPath, "images");
}
```

**Pobieranie listy plików:**

```csharp
private void UpdateFileList()
{
    Images = new List<string>();
    foreach (var item in Directory.EnumerateFiles(imagesDir).ToList())
    {
        Images.Add(Path.GetFileName(item));
    }
}

public void OnGet()
{
    UpdateFileList();
}
```

**Uwagi:**

- Utworzenie folderu `wwwroot/images/` do przechowywania obrazków
- `Path.Combine()` zapewnia zgodność separatorów ścieżek z systemem operacyjnym
- `Directory.EnumerateFiles()` zwraca wszystkie pliki w katalogu

### Zadanie 4.3: Wysyłanie plików na serwer

Implementacja funkcjonalności uploadu plików graficznych z automatyczną konwersją nazw.

**Model strony z właściwością pliku:**

```csharp
[BindProperty]
public IFormFile Upload { get; set; }
```

Atrybut `[BindProperty]` powoduje automatyczne przypisanie danych z formularza do właściwości.

**Obsługa przesyłania pliku (OnPost):**

```csharp
public IActionResult OnPost()
{
    if (Upload != null)
    {
        string extension = ".jpg";
        switch (Upload.ContentType)
        {
            case "image/png":
                extension = ".png";
                break;
            case "image/gif":
                extension = ".gif";
                break;
        }

        var fileName = Path.GetFileNameWithoutExtension(
            Path.GetRandomFileName()) + extension;

        using (var fs = System.IO.File.OpenWrite(
            Path.Combine(imagesDir, fileName)))
        {
            Upload.CopyTo(fs);
        }
    }
    return RedirectToAction("Index");
}
```

**Formularz HTML z Tag Helpers:**

```razor
<form asp-page="Upload" enctype="multipart/form-data" method="post">
    <input class="form-control" asp-for="Upload" type="file"
           accept=".jpg, .png, .jpeg, .gif, .bmp, .tif, .tiff|image/*" />
    <input class="form-control" type="submit" value="Upload" />
</form>
```

**Kluczowe elementy:**

- `enctype="multipart/form-data"` - wymagany do przesyłania plików
- `asp-for` - Tag Helper wiążący input z właściwością modelu
- `accept` - ograniczenie typów plików po stronie przeglądarki
- Losowa nazwa pliku zapobiega konfliktom

### Zadanie 4.4: Dodawanie odnośnika do uploadu

Dodanie nawigacji do strony przesyłania plików.

**Link z wykorzystaniem Tag Helpers:**

```razor
<a asp-page="Upload">Upload new file...</a>
```

Tag Helper `asp-page` automatycznie generuje poprawny URL zgodny z regułami routingu aplikacji.

### Zadanie 4.5: Widoki częściowe

Utworzenie widoku częściowego (Partial View) do wyświetlania miniatur obrazków.

**Zawartość \_Image.cshtml:**

```razor
@model string

<img src="~/images/@Model" width="300" class="img-thumbnail" />
```

- Znak `~` generuje względny odnośnik do `wwwroot/`
- `@Model` zawiera nazwę pliku (typ `string`)

**Wykorzystanie w Index.cshtml:**

```razor
@foreach (var item in Model.Images)
{
    <partial name="_Image" model="item" />
}
```

**Rozszerzenie - wyśrodkowanie obrazków:**

Zastosowanie klas Bootstrap do wyśrodkowania i układu kolumnowego:

```razor
<div class="container">
    <div class="row justify-content-center">
        @foreach (var item in Model.Images)
        {
            <div class="col-12 text-center mb-3">
                <partial name="_Image" model="item" />
            </div>
        }
    </div>
</div>
```

### Zadanie 4.6: Strona pojedynczego obrazka z routingiem

Implementacja strony wyświetlającej pełnowymiarowy obrazek z parametrem w URL.

**Konfiguracja routingu w dyrektywie @page:**

```razor
@page "{image}"
```

Parametr `{image}` w URL zostanie przekazany do modelu strony.

**Model strony Single.cshtml.cs:**

```csharp
[BindProperty(SupportsGet = true)]
public string Image { get; set; }
```

`SupportsGet = true` umożliwia bindowanie danych z parametrów GET.

**Walidacja istnienia pliku:**

```csharp
private string imagesDir;

public IActionResult OnGet()
{
    if (System.IO.File.Exists(Path.Combine(imagesDir, Image)))
    {
        return Page();
    }
    else
    {
        return NotFound();
    }
}
```

**Wyświetlanie obrazka (Single.cshtml):**

```razor
<img src="~/images/@Model.Image" class="img-fluid" />
```

**Modyfikacja widoku częściowego z linkami:**

```razor
@model string

<a asp-page="Single" asp-route-image="@Model">
    <img src="~/images/@Model" width="300" class="img-thumbnail" />
</a>
```

`asp-route-image` automatycznie generuje URL z parametrem zgodnym z routingiem.

### Zadanie 4.7: Znaki wodne (Magick.NET)

Dodanie automatycznego nanoszenia znaków wodnych na przesyłane obrazy.

**Przygotowanie:**

- Dodanie pliku `watermark.png` do głównego folderu projektu
- Plik będzie nakładany jako półprzezroczysty znak wodny

**Implementacja nakładania znaku wodnego:**

```csharp
using ImageMagick;

public IActionResult OnPost()
{
    if (Upload != null)
    {
        string extension = ".jpg";
        switch (Upload.ContentType)
        {
            case "image/png":
                extension = ".png";
                break;
            case "image/gif":
                extension = ".gif";
                break;
        }

        var fileName = Path.GetFileNameWithoutExtension(
            Path.GetRandomFileName()) + extension;
        var path = Path.Combine(imagesDir, fileName);

        // Wczytanie przesyłanego obrazu
        using var image = new MagickImage(Upload.OpenReadStream());

        // Wczytanie znaku wodnego
        using var watermark = new MagickImage("watermark.png");

        // Ustawienie przezroczystości (dzielenie przez 4)
        watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);

        // Nałożenie znaku wodnego w prawym dolnym rogu
        image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);

        // Zapis do pliku
        image.Write(path);
    }
    return RedirectToPage("Index");
}
```

**Kluczowe elementy:**

- `MagickImage` - klasa reprezentująca obraz
- `Evaluate()` - modyfikacja kanału alfa (przezroczystość)
- `Composite()` - nakładanie obrazu na obraz
- `Gravity.Southeast` - pozycja (prawy dolny róg)
- `CompositeOperator.Over` - tryb nakładania

### Zadanie 4.8: Walidacja rozmiaru pliku

Implementacja sprawdzania rozmiaru przesyłanego pliku (maksymalnie 1 MB).

**Sprawdzanie rozmiaru w OnPost():**

```csharp
public IActionResult OnPost()
{
    if (Upload != null)
    {
        // Sprawdzenie rozmiaru (1 MB = 1048576 bajtów)
        if (Upload.Length > 1048576)
        {
            ModelState.AddModelError("Upload",
                "Plik jest za duży (max. 1 MB)");
            return Page();
        }

        // ... reszta kodu uploadu
    }
    return RedirectToPage("Index");
}
```

**Wyświetlanie błędów w widoku Upload.cshtml:**

```razor
<form asp-page="Upload" enctype="multipart/form-data" method="post">
    <div asp-validation-summary="All" class="text-danger"></div>

    <input class="form-control" asp-for="Upload" type="file"
           accept=".jpg, .png, .jpeg, .gif, .bmp, .tif, .tiff|image/*" />
    <input class="form-control" type="submit" value="Upload" />
</form>
```

**Mechanizm walidacji:**

- `ModelState.AddModelError()` - dodanie błędu do stanu modelu
- `asp-validation-summary` - Tag Helper wyświetlający wszystkie błędy
- `return Page()` - powrót do formularza z zachowaniem stanu

## Uruchomienie

### Wymagania

- .NET 8.0 SDK lub nowszy
- Visual Studio 2022 / VS Code + C# Extension / Rider
- Przeglądarka internetowa

### Kompilacja i uruchomienie

```bash
dotnet build
dotnet run
```

Aplikacja będzie dostępna pod adresem: `http://localhost:5000`

## Routing i nawigacja

### Dostępne endpointy

- `/` lub `/Index` - strona główna (lista obrazków)
- `/Upload` - formularz przesyłania plików
- `/Single/{nazwa_pliku}` - pojedynczy obrazek w pełnym rozmiarze

### Domyślny routing Razor Pages

```
/Pages/NazwaStrony.cshtml → /nazwastrony
/Pages/Folder/Strona.cshtml → /folder/strona
```

Parametry można przekazywać przez URL: `/strona/{parametr}`

## 🔧 Wykorzystane technologie i koncepcje

### Razor Pages

- **PageModel** - model strony zawierający logikę biznesową
- **Code-behind** - oddzielenie logiki od prezentacji
- **Tag Helpers** - uproszczone generowanie HTML
- **Binding** - automatyczne mapowanie danych

### Obsługa plików

- **IFormFile** - reprezentacja przesyłanego pliku
- **Stream operations** - efektywna praca z danymi binarnymi
- **Path manipulation** - bezpieczne operacje na ścieżkach

### Dependency Injection

- **IWebHostEnvironment** - dostęp do ścieżek aplikacji
- `WebRootPath` - ścieżka do folderu wwwroot/
- Wstrzykiwanie przez konstruktor PageModel

### Widoki częściowe (Partial Views)

- Reużywalne fragmenty interfejsu
- Własny model danych
- Prefix `_` w nazwie pliku

### Routing

- Parametry w URL: `@page "{param}"`
- `[BindProperty(SupportsGet = true)]`
- `asp-route-{parametr}` - Tag Helper

### Walidacja

- `ModelState` - stan walidacji modelu
- `AddModelError()` - dodawanie błędów
- `asp-validation-summary` - wyświetlanie błędów

### Manipulacja obrazami

- **Magick.NET** - biblioteka do przetwarzania grafiki
- Nakładanie znaków wodnych
- Operacje na kanałach kolorów
- Kontrola przezroczystości

## Różnice między Razor Pages a MVC

| Aspekt       | Razor Pages                     | MVC                             |
| ------------ | ------------------------------- | ------------------------------- |
| Struktura    | Pages/                          | Controllers/ + Views/           |
| Organizacja  | Strona = widok + model          | Rozdzielone kontrolery i widoki |
| Routing      | Bazowany na strukturze folderów | Konfigurowalny w Program.cs     |
| Model        | PageModel (code-behind)         | Klasy modeli + ViewModels       |
| Zastosowanie | Scenariusze page-focused        | Złożone aplikacje, API          |

## Dodatkowe informacje

### System NuGet

NuGet to menedżer pakietów dla .NET:

- Repozytorium: https://nuget.org
- Automatyczne zarządzanie zależnościami
- Integracja z Visual Studio i narzędziami CLI

## Pytania kontrolne

1. **Czym różnią się aplikacje Razor Pages od aplikacji MVC?**

   - Razor Pages: jeden plik strony z logiką i widokiem, routing bazowany na folderach
   - MVC: oddzielne kontrolery i widoki, elastyczny routing

2. **Na czym polega routing w aplikacji internetowej?**

   - Mapowanie URL-i na konkretne akcje/strony
   - W Razor Pages: struktura folderów + parametry w `@page`
   - Możliwość przekazywania parametrów przez URL

3. **Jak obsługuje się ścieżki plików w aplikacjach .NET?**
   - `Path.Combine()` - łączenie segmentów ścieżki
   - `IWebHostEnvironment.WebRootPath` - ścieżka do wwwroot/
   - Znak `~` w Razor - odniesienie do wwwroot/

---

**Projekt wykonany w ramach zajęć laboratoryjnych z aplikacji internetowych ASP.NET Core.**

**Nie ma skrótów do miejsca, do którego warto dotrzeć" - nieznany** 🎓
