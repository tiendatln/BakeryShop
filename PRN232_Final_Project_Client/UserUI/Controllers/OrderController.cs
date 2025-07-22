using DTOs.CartDTO;
using DTOs.OrderDTO;
using DTOs.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Interfaces;
using Service.Services;
using System.Text.Json;

namespace UserUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService, IProductService productService, ICartService cartService)
        {
            _orderService = orderService;
            _productService = productService;
            _cartService = cartService;
        }

        // GET: /Order
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Common");

            // Calculate skip value for pagination
            int skip = (page - 1) * pageSize;

            var orders = await _orderService.GetOrdersAsync(token, skip, pageSize);

            // Send data using ViewBag
            ViewBag.Orders = orders;
            ViewBag.Token = token;
            ViewBag.CurrentPage = page;
            // ViewBag.TotalCount = totalCount;

            return View("History");
        }

        public async Task<IActionResult> HistoryDetail(string orderJson)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var order = System.Text.Json.JsonSerializer.Deserialize<ReadOrderDTO>(orderJson, options);

            // Tạo List<ReadProductDTO> để chứa thông tin sản phẩm
            var products = new List<ReadProductDTO>();
            var messages = new List<string>();

            // Lặp qua từng OrderDetail để lấy thông tin sản phẩm
            foreach (var detail in order.OrderDetails)
            {
                try
                {
                    Console.WriteLine($"Fetching product details for ProductID: {detail.ProductID}");
                    var product = await _productService.GetProductByIdAsync(detail.ProductID);

                    if (product != null)
                    {
                        products.Add(product);
                    }
                    else
                    {
                        // Tạo một ReadProductDTO giả để hiển thị lỗi
                        products.Add(new ReadProductDTO
                        {
                            ProductID = detail.ProductID,
                            ProductName = "[Sản phẩm không tồn tại]",
                            ImageURL = "/images/no-image.png",
                            Price = 0,
                            StockQuantity = 0
                        });
                        messages.Add($"❌ Sản phẩm có ID {detail.ProductID} không tồn tại.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"[ERROR] Product API failed for ID {detail.ProductID}: {ex.Message}");

                    // Tạo một ReadProductDTO giả để hiển thị lỗi
                    products.Add(new ReadProductDTO
                    {
                        ProductID = detail.ProductID,
                        ProductName = "[Lỗi khi lấy sản phẩm]",
                        ImageURL = "/images/no-image.png",
                        Price = 0,
                        StockQuantity = 0
                    });
                    messages.Add($"❌ Lỗi khi lấy thông tin sản phẩm ID {detail.ProductID}: {ex.Message}");
                }
            }

            ViewBag.Order = order;
            ViewBag.Products = products;
            ViewBag.ProductMessages = messages;

            return View("HistoryDetail", order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(string fullName, string email, string phone, string shipping_address, decimal totalPrice, string cartIds, string paymentMethod, string cartJson)
        {
            if (string.IsNullOrEmpty(shipping_address))
            {
                ModelState.AddModelError("shipping_address", "Shipping address is required.");
                return RedirectToAction("Checkout");
            }

            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Common");

            var cartIdList = cartIds.Split(',').Select(int.Parse).ToList();

            var newOrder = new DTOs.OrderDTO.CreateOrderDTO
            {
                OrderDate = DateTime.UtcNow,
                ShippingAddress = shipping_address,
                TotalAmount = totalPrice,
                OrderStatus = "Pending",
                PaymentMethod = paymentMethod,
                PaymentStatus = "Pending",
            };

            var orderId = await _orderService.CreateOrderAsync(newOrder, token);

            if (orderId != null)
            {
                var carts = JsonConvert.DeserializeObject<List<CartDTO>>(cartJson);

                foreach (var cart in carts)
                {
                    var product = await _productService.GetProductByIdAsync(cart.ProductID);
                    var unitPrice = product?.Price ?? 0;
                    var newQuantity = product.StockQuantity - cart.Quantity;

                    var detailDto = new CreateOrderDetailDTO
                    {
                        OrderID = orderId.Value,
                        ProductID = cart.ProductID,
                        Quantity = cart.Quantity,
                        UnitPrice = unitPrice, 
                        TotalPrice = unitPrice * cart.Quantity,
                    };

                    await _orderService.CreateOrderDetailAsync(detailDto, orderId.Value, token); // Gọi để tạo OrderDetail



                    // update product stock quantity
                    await _productService.UpdateQuantityAsync(product.ProductID, newQuantity, token);

                    // delete cart item after order created
                    await _cartService.DeleteCartAsync(cart.CartID, token);
                }

                TempData["OrderSuccess"] = "Order created successfully!";
                return RedirectToAction("Index", "Home");
            }

            TempData["OrderErrors"] = "Failed to create order. Please try again.";
            return RedirectToAction("Checkout");
        }

    }
}
