using CartAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UserUI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Common");

            var carts = await _cartService.GetCartAsync(token);
            return View(carts);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartCreateDTO dto)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var success = await _cartService.AddCartAsync(dto, token);
            if (!success) ModelState.AddModelError("", "Add to cart failed!");

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            await _cartService.DeleteCartAsync(id, token);
            return RedirectToAction("Index");
        }

        // POST: /Cart/Update
        [HttpPost]
        public async Task<IActionResult> Update(List<CartQuantityUpdateDTO> updates)
        {
            var token = HttpContext.Session.GetString("UserToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            await _cartService.UpdateQuantitiesAsync(updates, token);
            return RedirectToAction("Index");
        }
    }
}
