using Microsoft.AspNetCore.Mvc;
using Services.Shop;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            var products = _productService.GetMany(s => true, null).ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _productService.GetOne(s => s.ID == id, null);
            if(product is null)
                return NotFound();

            return View(product);
        }

    }
}
