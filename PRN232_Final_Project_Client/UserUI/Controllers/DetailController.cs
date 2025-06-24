using DTOs.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

public class DetailController : Controller
{
    private readonly IProductService _productService;

    public DetailController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);

        if (product == null)
            return NotFound();

        // Giả sử ông có method để lấy sản phẩm tương tự theo category hoặc logic nào đó
        var similarProducts = await _productService.GetProductsByCategoryAsync(product.CategoryID);

        ViewBag.Product = product;
        ViewBag.SimilarProducts = similarProducts ?? new List<ReadProductDTO>(); // tránh null

        return View();
    }
}
