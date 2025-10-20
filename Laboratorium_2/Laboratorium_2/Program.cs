using Laboratorium_2;
using System.Linq;

List<Fruit> fruits = new List<Fruit>();

for (int i = 0; i < 15; i++)
{
    fruits.Add(Fruit.Create());
}

Console.WriteLine("\n=== Zadanie 2.4 ===");
await Zadanie2_4();

Console.WriteLine("=== Zadanie 2.2 ===");
Zadanie2_2(fruits);

Console.WriteLine("\n=== Zadanie 2.3 ===");
Zadanie2_3(fruits);


static void Zadanie2_2(List<Fruit> fruits)
{
    foreach (var fruit in fruits)
    {
        Console.WriteLine(fruit);
    }
}

static void Zadanie2_3(List<Fruit> fruits)
{
    var sweetFruits = fruits
        .Where(f => f.IsSweet)
        .OrderByDescending(f => f.Price);

    foreach (var fruit in sweetFruits)
    {
        Console.WriteLine(fruit);
    }
}

static async Task Zadanie2_4()
{
    try
    {
        UsdCourse.Current = await UsdCourse.GetUsdCourseAsync();
        Console.WriteLine($"Aktualny kurs USD: {UsdCourse.Current} zł");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Błąd podczas pobierania: {ex.Message}");
    }
}