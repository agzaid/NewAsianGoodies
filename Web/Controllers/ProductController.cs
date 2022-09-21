//using AutoMapper;
//using Data.Entities.Shop;
//using Microsoft.AspNetCore.Mvc;
//using Services.Injection;
//using Services.Shop;
//using Web.Models.Shop;
//using System.Linq.Dynamic.Core;
//using System.Linq;

//namespace Web.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ProductController : Controller
//    {
//        private readonly IProductService _productService;
//        private readonly IMapper _mapper;

//        public ProductController(IProductService productService, IMapper mapper)
//        {
//            _productService = productService;
//            _mapper = mapper;
//        }

//        public IActionResult Index()
//        {
//            var products = _productService.GetMany(s => true, null).ToList();
//            var recordsTotal = products.Count();
//            ViewData["records"] = recordsTotal;
//            return View(products);
//        }
//        [HttpPost]
//        public IActionResult LoadProducts()
//        {
//            var pageSize = int.Parse(Request.Form["length"]);
//            var skip = int.Parse(Request.Form["start"]);
//            var searchValue = Request.Form["search[value]"];
//            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][name]"];
//            var sortDir = Request.Form["order[0][dir]"];

//            //for searching
//            IEnumerable<Product> products = _productService.GetMany(s => true, null)
//                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.Description.Contains(searchValue) || m.Price.ToString().Contains(searchValue)));

//            //for sorting
//            //IQueryable<Product> queryProducts = (IQueryable<Product>)products;
//            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDir)))
//            //   products = queryProducts.OrderBy(string.Concat(sortColumn, " ", sortDir));

//            var data = products.Skip(skip).Take(pageSize).ToList();

//            var recordsTotal = products.Count();
//            ViewData["records"] = recordsTotal;
//            return Ok(new { recordsFiltered = recordsTotal, recordsTotal, data = data });
//        }

//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(ProductViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                model.ImageFiles.ForEach(async i => model.Images.Add($"/Uploads/Products/{await FileExtension.CreateFile(i, "Products")}"));
//                var product = new Product()
//                {
//                    ID = 0,
//                    Description = model.Description,
//                    Name = model.Name,
//                    Price = model.Price,
//                    //Image= model.Images
//                };
//                _productService.Insert(product);
//            }
//            return RedirectToAction("Index");
//        }
//        public IActionResult Edit(int? id)
//        {
//            if (id == null || id == 0)
//            {
//                return NotFound();
//            }
//            var product = _productService.GetOne(s => s.ID == id, null);
//            var productViewModel = _mapper.Map<ProductViewModel>(product);
//            if (product == null)
//            {
//                return NotFound();
//            }
//            return View(productViewModel);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(ProductViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var oldModel = _productService.GetOne(s => s.ID == model.Id, null);

//                oldModel.Name = model.Name;
//                oldModel.Description = model.Description;
//                oldModel.Price = model.Price;

//                _productService.Update(oldModel);
//                return RedirectToAction("Index");
//            }
//            return View(model);
//        }
//        [HttpGet]
//        public IActionResult Delete(int? id)
//        {
//            if (id == null || id == 0)
//            {
//                return NotFound();
//            }
//            var category = _productService.GetOne(s => s.ID == id, null);
//            if (category == null)
//            {
//                return NotFound();
//            }
//            return View(category);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Delete(int id)
//        {
//            _productService.Delete(id);

//            return RedirectToAction("Index");
//        }


//    }
//}
