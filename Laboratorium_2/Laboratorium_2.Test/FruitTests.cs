using Laboratorium_2;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Laboratorium_2.Tests
{
    public class FruitTests
    {
        [Fact]
        public void Fruit_ToString_ShouldReturnProperFormat()
        {
            var fruit = new Fruit
            {
                Name = "Wiœnia",
                IsSweet = true,
                Price = 5.50
            };

            UsdCourse.Current = 4.00F;

            var result = fruit.ToString();

            string expectedPricePL = fruit.Price.ToString("C2", new CultureInfo("pl-PL"));
            string expectedPriceUSD = Fruit.FormatUsdPrice(fruit.UsdPrice);

            string expected =
                $"Fruit: Name=Wiœnia, IsSweet=true, Price={expectedPricePL}, UsdPrice={expectedPriceUSD}";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Fruit_Create_ShouldGenerateDifferentNames()
        {
            var names = new List<string>();

            for (int i = 0; i < 20; i++)
            {
                var fruit = Fruit.Create();
                names.Add(fruit.Name);
            }

            int uniqueNamesCount = names.Distinct().Count();

            Assert.True(uniqueNamesCount > 1,
                $"Oczekiwano wiêcej ni¿ 1 unikaln¹ nazwê, ale znaleziono {uniqueNamesCount}");
        }
    }
}
