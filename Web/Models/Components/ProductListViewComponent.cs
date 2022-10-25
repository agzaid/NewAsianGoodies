using Microsoft.AspNetCore.Mvc;
using Services.Shop;

namespace Web.Models.Components
{
    public class ProductListViewComponent :ViewComponent
    {
        private readonly IProductService _productService;

        public ProductListViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _productService.GetMany(s => true, null).ToList();
            return View(model);
        }
    }
}
