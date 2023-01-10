using ContosoCrafts.WebSites.Models;
using System.CodeDom.Compiler;
using System.Text.Json;

namespace ContosoCrafts.WebSites.Services
{
    public class JsonFileProductService
    {

        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");

        public IEnumerable<Product> GetProducts()
        {

            using var jsonFileReader = File.OpenText(JsonFileName);
            

            var json= JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            jsonFileReader.Close();
            return json;

        }
        public void AddRatings(string productID, int rating)
        {
            var products = GetProducts();
            if (products.First(x => x.Id == productID).Ratings == null)
            {
                products.First(x => x.Id == productID).Ratings = new[] { rating };
            }
            else
            {
                var ratings = products.First(x => x.Id == productID).Ratings.ToList();
                ratings.Add(rating);
                products.First(x => x.Id == productID).Ratings = ratings.ToArray();
            }
            using var outputStream = File.OpenWrite(JsonFileName);

            JsonSerializer.Serialize<IEnumerable<Product>>(
                new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    SkipValidation = true,
                    Indented = true
                }),
                products);
        }

    }
}
