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

        // GET: ProductController/Create
        //public async Task<ActionResult> Create(CreateProductDTO product, IFormFile img)
        //{
        //    if (img != null && img.Length > 0)
        //    {
        //        // Save the image to a specific path, e.g., wwwroot/images
        //        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img/product");
        //        Directory.CreateDirectory(folderPath);
        //        var fileName = Path.GetFileName(img.FileName);
        //        var filePath = Path.Combine(folderPath, fileName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await img.CopyToAsync(stream); // Save the uploaded file
        //        }
        //        product.ImageURL = $"img/product/{fileName}"; // Set the image URL
        //    }
        //    else
        //    {
        //        product.ImageURL = string.Empty; // Set default or empty if no image provided
        //    }
        //    await _productService.CreateProductAsync(product).ContinueWith(task =>
        //    {
        //        if (task.IsCompletedSuccessfully)
        //        {
        //            ViewBag.Message = "Product created successfully.";
        //        }
        //        else
        //        {
        //            ViewBag.Error = "Failed to create product.";
        //        }
        //    });
        //    return View();
        //}



        // GET: ProductController/Edit/5
        //public async Task<ActionResult> Edit(UpdateProductDTO product, IFormFile img)
        //{
        //    if (img != null && img.Length > 0)
        //    {
        //        // Save the image to a specific path, e.g., wwwroot/images
        //        var existingProduct = await _productService.GetProductByIdAsync(product.ProductID);
        //        var folderPathExis = Path.Combine(Directory.GetCurrentDirectory());
        //        var filePathExis = Path.Combine(folderPathExis, existingProduct.ImageURL);


        //        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "img");
        //        Directory.CreateDirectory(folderPath);
        //        var fileName = Path.GetFileName(img.FileName);
        //        var filePath = Path.Combine(folderPath, fileName);

        //        if (System.IO.File.Exists(filePathExis))
        //        {
        //            System.IO.File.Delete(filePathExis); // Xóa file cũ nếu tồn tại
        //        }

        //        // Lưu file mới
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await img.CopyToAsync(stream);
        //        }

        //        product.ImageURL = $"img/{fileName}"; // Set the image URL
        //    }
        //    else
        //    {
        //        product.ImageURL = string.Empty; // Set default or empty if no image provided
        //    }
        //    await _productService.GetProductByIdAsync(product.ProductID).ContinueWith(task =>
        //    {
        //        if (task.IsCompletedSuccessfully)
        //        {
        //            ViewBag.Product = task.Result;
        //        }
        //        else
        //        {
        //            ViewBag.Error = "Failed to load product for editing.";
        //        }
        //    });
        //    return View();
        //}



        // GET: ProductController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    _productService.DeleteProductAsync(id).ContinueWith(task =>
        //    {
        //        if (task.IsCompletedSuccessfully)
        //        {
        //            ViewBag.Message = "Product deleted successfully.";
        //        }
        //        else
        //        {
        //            ViewBag.Error = "Failed to delete product.";
        //        }
        //    });
        //    return View();
        //}

        
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
