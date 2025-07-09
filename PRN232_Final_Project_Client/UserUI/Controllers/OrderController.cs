using DTOs.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Interfaces;

namespace UserUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
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

        /*[HttpGet]
        public async Task<IActionResult> GetOrdersPaginated(int page = 1, int pageSize = 8)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Unauthorized" });

            try
            {
                // Calculate skip value
                int skip = (page - 1) * pageSize;

                // Get orders for current page
                var orders = await _orderService.GetOrdersAsync(token, skip, pageSize);

                return Json(new
                {
                    success = true,
                    orders = orders,
                    currentPage = page,
                    pageSize = pageSize,
                    hasMore = orders.Count == pageSize 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }*/

        // GET: /Order/Details/{orderId}
        public async Task<IActionResult> Details(int orderId, int page = 1, int pageSize = 8)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Common");

            int skip = (page - 1) * pageSize;

            var orderDetail = await _orderService.GetOrderDetailAsync(orderId, token, page, pageSize);
            if (orderDetail == null) return NotFound();

            // get list product by orderDetail
            var products = new List<ReadProductDTO>();
            foreach (var detail in orderDetail)
            {
                var product = await _productService.GetProductByIdAsync(detail.ProductID);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            // send order using ViewBag
            ViewBag.OrderDetails = orderDetail;
            ViewBag.Products = products;
            ViewBag.OrderId = orderId;

            return View("HistoryDetail");
        }
    }
}
