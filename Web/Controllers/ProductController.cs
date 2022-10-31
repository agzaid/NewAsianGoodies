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
        public async Task<IActionResult> Index()
        {
            var products =  await _productService.GetMany(s => true, null);
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
