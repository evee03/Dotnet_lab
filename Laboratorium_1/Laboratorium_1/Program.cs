using Laboratorium_1;
using System.Text.Json;
static void zadanie1_3()
{
    Console.WriteLine("Zadanie 1.1");
    for (int i = 1; i <= 100; i++)
    {
        if (i % 3 == 0 && i % 5 == 0)
            Console.WriteLine("FizzBuzz");
        else if (i % 3 == 0)
            Console.WriteLine("Fizz");
        else if (i % 5 == 0)
            Console.WriteLine("Buzz");
        else
            Console.WriteLine(i);
    }
}

static void zadanie1_4() 
{
    const string FileName = "highscores.json";
    List<HighScore> highScores;

    if (File.Exists(FileName))
    {
        highScores = JsonSerializer.Deserialize<List<HighScore>>(File.ReadAllText(FileName));
    }
    else
    {
        highScores = new List<HighScore>();
    }

    var rand = new Random();
    var value = rand.Next(1, 101); 

    int guess = 0;
    int attempts = 0; 

    Console.WriteLine("Zgadnij liczbę od 1 do 100:");

    while (guess != value)
    {
        Console.Write("Podaj swoją liczbę: ");
        guess = Convert.ToInt32(Console.ReadLine());
        attempts++; 

        if (guess < value)
            Console.WriteLine("Za mało");
        else if (guess > value)
            Console.WriteLine("Za dużo");
        else
            Console.WriteLine($"Zgadłeś liczbę w {attempts} próbach");
    }

    Console.Write("Podaj swoje imię: ");
    string name = Console.ReadLine();

    var hs = new HighScore { Name = name, Trials = attempts };
    highScores.Add(hs);

    File.WriteAllText(FileName, JsonSerializer.Serialize(highScores));

    highScores.Sort((a, b) => a.Trials.CompareTo(b.Trials));

    Console.WriteLine("\nNajlepsze wyniki:");
    foreach (var item in highScores)
    {
        Console.WriteLine($"{item.Name} — {item.Trials} prób");
    }

}

zadanie1_4();