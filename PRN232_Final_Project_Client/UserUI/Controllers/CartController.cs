using CartAPI.DTOs;
using DTOs.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UserUI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }
        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Common");

            var carts = await _cartService.GetCartAsync(token);

            // Lấy danh sách product theo thứ tự từng cart
            var products = new List<ReadProductDTO>();
            foreach (var cart in carts)
            {
                var product = await _productService.GetProductByIdAsync(cart.ProductID);
                products.Add(product);
            }

            // Gửi qua ViewBag (không cần tạo ViewModel)
            ViewBag.CartItems = carts;
            ViewBag.Products = products;


            foreach (var cart in carts)
            {
                var product = await _productService.GetProductByIdAsync(cart.ProductID);
                products.Add(product);

                Console.WriteLine($"CartID: {cart.CartID}, ProductID: {cart.ProductID}, Quantity: {cart.Quantity}");
                Console.WriteLine($"→ Product: {product.ProductName}, Price: {product.Price}");
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartCreateDTO dto)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var success = await _cartService.AddCartAsync(dto, token);
            if (!success)
            {
                TempData["OrderErrors"] = "Add to cart failed!";
            }

            return RedirectToAction("Index"); // hoặc quay lại Detail nếu muốn
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartID)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            await _cartService.DeleteCartAsync(cartID, token);
            return Ok();
        }

        // POST: /Cart/Update
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(List<CartQuantityUpdateDTO> updates)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            await _cartService.UpdateQuantitiesAsync(updates, token);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(List<int> cartIds)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Common");

            if (cartIds == null || cartIds.Count == 0)
            {
                TempData["OrderErrors"] = "No cart selected!";
                return RedirectToAction("Index");
            }

            // Lấy chi tiết cart
            var allCarts = await _cartService.GetCartAsync(token);
            var selectedCarts = allCarts.Where(c => cartIds.Contains(c.CartID)).ToList();

            // Lấy danh sách sản phẩm tương ứng
            var selectedProducts = new List<ReadProductDTO>();
            foreach (var cart in selectedCarts)
            {
                var product = await _productService.GetProductByIdAsync(cart.ProductID);
                selectedProducts.Add(product);
            }

            // Gửi qua View
            ViewBag.CartItems = selectedCarts;
            ViewBag.Products = selectedProducts;
            ViewBag.CartIDs = cartIds;

            return View("Checkout"); // trả về trang Checkout.cshtml
        }

    }
}
