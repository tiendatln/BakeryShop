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

        


        public async Task<IActionResult> SearchProductsAjax(string searchTerm, int categoryId)
        {
            if (!string.IsNullOrEmpty(searchTerm) && categoryId > 0)
            {
                var product = await _productService.GetProductsByCategoryAsync(categoryId);
                var newPro = product.Where(p => p.ProductName.Contains(searchTerm));
                return Json(newPro); // Trả JSON danh sách sản phẩm theo danh mục
            }
            var products = await _productService.SearchProductsAsync(searchTerm);
            return Json(products); // Trả JSON danh sách sản phẩm
        }

        public async Task<ActionResult> GetProductsByCategory(int categoryId)
        {
            var product = await _productService.GetProductsByCategoryAsync(categoryId);

            return Json(product); // Redirect to Index view with category products
        }
    }
}
