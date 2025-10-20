using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Laboratorium_2
{
    public class UsdCourse
    {
        public static float Current = 0;

        public async static Task<float> GetUsdCourseAsync()
        {
            var wc = new HttpClient();
            var response = await wc.GetAsync("https://api.nbp.pl/api/exchangerates/tables/a/?format=xml");

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Nie udało się pobrać kursu.");

            string xmlContent = await response.Content.ReadAsStringAsync();

            XDocument doc = XDocument.Parse(xmlContent);

            var midUsdValue = doc.Descendants("Rate")
                .Where(rate => (string)rate.Element("Code") == "USD")
                .Select(rate => (string)rate.Element("Mid"))
                .FirstOrDefault();

            if (midUsdValue == null)
                throw new InvalidOperationException("Nie znaleziono kursu USD.");

            return Convert.ToSingle(midUsdValue, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
