using DTOs.ProductDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Threading.Tasks;

namespace UserUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            //var products = await _productService.GetAllProductsAsync();
                

            //ViewBag.Products = products;
            return View();
        }
        public async Task<JsonResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Json(products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            _productService.GetProductByIdAsync(id).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    ViewBag.Product = task.Result;
                }
                else
                {
                    ViewBag.Error = "Failed to load product details.";
                }
            });
            return View();
        }




        public async Task<IActionResult> SearchProducts(string searchTerm, int categoryId, double minPrice, double maxPrice, int pageNumper)
        {

            var search = await _productService.SearchProductsOdataAsync(searchTerm, categoryId, 0, minPrice, maxPrice, 0, 0);
            var totalPages = (int)Math.Ceiling(search.Count / 12f);

            var product = await _productService.SearchProductsOdataAsync(searchTerm, categoryId, 0, minPrice, maxPrice, 12, (pageNumper - 1) * 12);
            return Json(new
            {
                products = product,
                totalPages = totalPages
            });
        }

        public async Task<ActionResult> GetProductsByCategory(int categoryId)
        {
            var product = await _productService.GetProductsByCategoryAsync(categoryId);

            return Json(product); // Redirect to Index view with category products
        }

        public async Task<JsonResult> SearchSuggestions(string searchTerm, int categoryId, double minPrice, double maxPrice)
        {
            var search = await _productService.SearchProductsOdataAsync(searchTerm, categoryId, 0, minPrice, maxPrice, 4, 0);
            return Json(search);
        }
    }
}
