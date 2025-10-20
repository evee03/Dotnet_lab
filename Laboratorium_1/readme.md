# Laboratorium 1 - Podstawy C#

Laboratorium zawiera implementację podstawowych zagadnień programowania w C# na platformie .NET, obejmujący pętle, instrukcje warunkowe oraz pracę z plikami JSON.

## Zawartość 

- **Zadanie 1.3** - FizzBuzz
- **Zadanie 1.4** - Gra w zgadywanie liczb
- **Zadanie 1.5** - Licznik prób
- **Zadanie 1.6** - System najlepszych wyników

## Opis zadań

### Zadanie 1.3: FizzBuzz
Program wyświetla liczby od 1 do 100 z następującymi zasadami:
- Dla liczb podzielnych przez 3 → wyświetla "Fizz"
- Dla liczb podzielnych przez 5 → wyświetla "Buzz"
- Dla liczb podzielnych przez 3 i 5 → wyświetla "FizzBuzz"
- Dla pozostałych liczb → wyświetla ich wartość

**Przykładowy wynik:**
```
1
2
Fizz
4
Buzz
Fizz
7
8
Fizz
Buzz
11
Fizz
13
14
FizzBuzz
```

### Zadanie 1.4: Gra w zgadywanie
Interaktywna gra, w której:
- Program losuje liczbę z zakresu 1-100
- Użytkownik próbuje ją zgadnąć
- Program podpowiada czy próba jest za duża lub za mała
- Gra kończy się po prawidłowym zgadnięciu

### Zadanie 1.5: Licznik prób
Rozszerzenie gry o mechanizm zliczający liczbę prób potrzebnych do wygrania.

### Zadanie 1.6: System high scores
Implementacja systemu zapisywania najlepszych wyników:
- Zapisywanie imienia gracza i liczby prób
- Przechowywanie wyników w formacie JSON
- Wyświetlanie listy najlepszych wyników
- Automatyczne sortowanie od najlepszego wyniku


## Uruchomienie

### Wymagania
- .NET SDK (wersja zgodna z projektem)
- Visual Studio / Visual Studio Code / Rider (opcjonalnie)

### Kompilacja i uruchomienie
```bash
dotnet build
dotnet run
```

Lub w Visual Studio: **F5** (Start Debugging)

## Format danych

Wyniki są zapisywane w pliku `highscores.json` w następującym formacie:
```json
[
  {
    "Name": "Jan",
    "Trials": 5
  },
  {
    "Name": "Anna",
    "Trials": 7
  }
]
```

## 🔧 Wykorzystane technologie i koncepcje

- **Pętle**: `for`, `while`, `foreach`
- **Instrukcje warunkowe**: `if`, `else if`, `else`
- **Operator modulo**: `%` (sprawdzanie podzielności)
- **Klasa Random**: generowanie liczb losowych
- **Właściwości (Properties)**: automatyczne gettery i settery
- **Serializacja JSON**: `System.Text.Json`
- **Obsługa plików**: `File.Exists()`, `File.ReadAllText()`, `File.WriteAllText()`
- **Listy generyczne**: `List<T>`
- **Inferencja typów**: słowo kluczowe `var`

## Dodatkowe informacje

Zadania wykonane w ramach zajęć laboratoryjnych z programowania i architektury w .NET.

---

**Będzie łatwo! Pięć minut! Pomnożone przez stałą oczywiście. -- Daniel Saffioti 🎓**