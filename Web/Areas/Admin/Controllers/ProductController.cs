using AutoMapper;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Mvc;
using Services.Injection;
using Services.Shop;
using Web.Models.Shop;
using System.Linq.Dynamic.Core;
using System.Linq;
using Web.Areas.Admin.Models.Shop;
using System.Text.Json;
using Web.Areas.Admin.Models.Shop.product;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public IActionResult Index(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            var products = _productService.GetMany(s => true, new List<string>() { "Category" });

            

            var columns = new List<string>()
            {
                "Name",
                "Price",
                "DisplayOrder",
                "Quantity",
                "ShortDescription",
                "Status"
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;

            return View(products);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var product = new CreateProductViewModel();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateProductViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    ThumbnailImage = $"/Uploads/Products/{await model.ThumbnailFormFile.CreateFile("Products")}",
                    CreatedDate = DateTime.UtcNow,
                    DisplayOrder = model.DisplayOrder,
                    ModifiedDate = DateTime.UtcNow,
                    Price = model.Price,
                    ProductName = model.ProductName,
                    Quantity = model.Quantity,
                    ShortDescription = model.ShortDescription,
                    Status = model.Status,
                };
                _productService.Insert(product);
                Message.Add("Success");
            }
            return RedirectToAction("index", new { message = Message });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = await _productService.GetOne(s => s.ID == id, null);
            var productViewModel = _mapper.Map<CreateProductViewModel>(product);

            if (product == null)
            {
                return NotFound();
            }
            return View(productViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(CreateProductViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var oldModel = await _productService.GetOne(s => s.ID == model.ID, null);

                if (model.ThumbnailFormFile is not null)
                {
                    FileExtension.DeleteFile(oldModel.ThumbnailImage);
                    oldModel.ThumbnailImage = $"/Uploads/Products/{await model.ThumbnailFormFile.CreateFile("Products")}";
                }

                oldModel.ProductName = model.ProductName;
                oldModel.ShortDescription = model.ShortDescription;
                oldModel.Price = model.Price;
                oldModel.Status = model.Status;
                oldModel.Quantity = model.Quantity;
                oldModel.DisplayOrder = model.DisplayOrder;

                _productService.Update(oldModel);

                Message.Add("Create Success");
                return RedirectToAction("Index", new { message = Message });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var Message = new List<string>();
            var product = await _productService.GetOne(s => s.ID == id, null);
            var result = FileExtension.DeleteFile(product.ThumbnailImage);

            if (!result.Succeeded)
            {
                Message.AddRange(result.Errors);
                return RedirectToAction("Index", new { message = Message });
            }
            _productService.Delete(id);

            return RedirectToAction("Index", new { message = Message });
        }


        #region Helper Methods

        [HttpPost]
        public IActionResult LoadDataTable()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][name]"];
            var sortDir = Request.Form["order[0][dir]"];

            //for searching
            IEnumerable<Product> products = _productService.GetMany(s => true, null)
                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.ProductName.Contains(searchValue) || m.ShortDescription.Contains(searchValue) || m.Price.ToString().Contains(searchValue)));

            var model = products.Select(s => new IndexProductViewModel()
            {
                ID = s.ID,
                Name = s.ProductName,
                DisplayOrder = s.DisplayOrder,
                Quantity = s.Quantity,
                Status = s.Status,
                ThumbnailImage = s.ThumbnailImage,
                ShortDescription = s.ShortDescription,
            });
            //for sorting
            //IQueryable<Product> queryProducts = (IQueryable<Product>)products;
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDir)))
            //   products = queryProducts.OrderBy(string.Concat(sortColumn, " ", sortDir));

            var data = model.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = model.Count();
            ViewData["records"] = recordsTotal;
            return Ok(new { recordsFiltered = recordsTotal, recordsTotal, data = data });
        }

        #endregion


    }
}
