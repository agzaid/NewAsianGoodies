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

        public IActionResult Index()
        {
            var products = _productService.GetMany(s => true, null).ToList();
            //var recordsTotal = products.Count();
            //ViewData["records"] = recordsTotal;
            var columns = new List<string>()
            {
                "Name",
                "Price",
                "DisplayOrder",
                "Quantity",
                "Status",
                //"ThumbnailImage",
                //"Image",
                "ShortDescription"
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
                //   model.ImageFiles.ForEach(async i => model.Images.Add($"/Uploads/Products/{await FileExtension.CreateFile(i, "Products")}"));
                //var product = new Product()
                //{
                //    ID = 0,
                //    Description = model.Description,
                //    Name = model.ProductName,
                //    Price = model.Price,
                //    //Image= model.Images
                //};
                //_productService.Insert(product);
                TempData["alertNotification"] = "success";
            }
            return RedirectToAction("index"); ;
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = _productService.GetOne(s => s.ID == id, null);
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
            if (ModelState.IsValid)
            {
                var oldModel = _productService.GetOne(s => s.ID == model.ID, null);

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
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _productService.GetOne(s => s.ID == id, null);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);

            return RedirectToAction("Index");
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

            //for sorting
            //IQueryable<Product> queryProducts = (IQueryable<Product>)products;
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDir)))
            //   products = queryProducts.OrderBy(string.Concat(sortColumn, " ", sortDir));

            var data = products.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = products.Count();
            ViewData["records"] = recordsTotal;
            return Ok(new { recordsFiltered = recordsTotal, recordsTotal, data = data });
        }

        #endregion


    }
}
