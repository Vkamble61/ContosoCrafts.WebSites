using ContosoCrafts.WebSites.Models;
using ContosoCrafts.WebSites.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSites.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController(JsonFileProductService productService)
        {
            this.ProductService = productService;
        }
        JsonFileProductService ProductService { get; set; }
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductService.GetProducts();
        }

        [Route("Rate")]
        //[HttpPatch]        [FromBody]
        [HttpGet]
        //https://localhost:7004/products/rate?ProductId=jenlooper-light&Ratings=3
        public ActionResult Get([FromQuery] string productID,
                               int ratings)
        {
            ProductService.AddRatings(productID, ratings);
            return Ok();
        }
    }
}
