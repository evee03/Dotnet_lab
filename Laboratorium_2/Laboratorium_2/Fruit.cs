using System;
using System.Globalization;
using System.Linq;

namespace Laboratorium_2
{
    public class Fruit
    {
        public string Name { get; set; }
        public bool IsSweet { get; set; }
        public double Price { get; set; }

        public double UsdPrice => UsdCourse.Current > 0
            ? Price / UsdCourse.Current
            : double.NaN;

        public static string FormatUsdPrice(double price)
        {
            var usCulture = new CultureInfo("en-US");
            return price.ToString("C2", usCulture);
        }

        public static Fruit Create()
        {
            Random r = new Random();
            string[] names = new string[]
            {
                "Ananas", "Czereśnia", "Jabłko", "Truskawka",
                "Porzeczka", "Wiśnia", "Jeżyna", "Malina",
                "Gruszka", "Brzoskwinia"
            };

            return new Fruit
            {
                Name = names[r.Next(names.Length)],
                IsSweet = r.NextDouble() > 0.5,
                Price = Math.Round(r.NextDouble() * 10, 2)
            };
        }

        public override string ToString()
        {
            string priceFormatted = Price.ToString("C2", new CultureInfo("pl-PL"));
            string usdFormatted = double.IsNaN(UsdPrice)
                ? "kurs USD nieznany"
                : FormatUsdPrice(UsdPrice);

            return $"Fruit: Name={Name}, IsSweet={IsSweet.ToString().ToLower()}, Price={priceFormatted}, UsdPrice={usdFormatted}";
        }
    }
}
