using System.Security.Claims;
using CartAPI.DTOs;
using CartAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;


namespace CartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        // GET: api/cart/user/5
        [HttpGet("{userId}")]
        [EnableQuery]
        public ActionResult<IQueryable<CartDTO>> GetCartsByUserIdOData(int userId)
        {
            var query = _service.GetCartQueryableByUserId(userId);
            return Ok(query);
        }
        //[HttpGet("me")]
        //[EnableQuery]
        //public ActionResult<IQueryable<CartDTO>> GetCartsByUserIdOData()
        //{

        //    foreach (var header in Request.Headers)
        //    {
        //        Console.WriteLine($"{header.Key}: {header.Value}");
        //    }

        //    if (!Request.Headers.TryGetValue("X-User-Id", out var userIdHeader))
        //    {
        //        return BadRequest(new { message = "User ID header missing." });
        //    }

        //    if (!int.TryParse(userIdHeader, out var userId))
        //    {
        //        return BadRequest(new { message = "Invalid user ID format in header." });
        //    }

        //    var query = _service.GetCartQueryableByUserId(userId);
        //    return Ok(query);
        //}
        //[HttpGet("{userId}")]
        //[EnableQuery]
        //public ActionResult<IQueryable<CartDTO>> GetCartsByUserIdOData(int userId)
        //{
        //    var query = _service.GetCartQueryableByUserId(userId);
        //    return Ok(query);
        //}

        //[HttpGet]
        //[EnableQuery]
        //public ActionResult<IQueryable<CartDTO>> GetCartsByUserIdOData([FromQuery] int userId)
        //{
        //    Console.WriteLine($"Received userId = {userId}");

        //    if (userId <= 0)
        //        return BadRequest(new { message = "Invalid user ID" });

        //    var query = _service.GetCartQueryableByUserId(userId);
        //    return Ok(query);
        //}


        // POST: api/cart
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddCart(int userId, [FromBody] CartCreateDTO dto)
        {
            dto.UserID = userId; // gán userId từ Ocelot route
            await _service.AddCart(dto);
            return Ok(new { message = "Cart item added successfully" });
        }

        // DELETE: api/cart/3
        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteCart(int cartId)
        {
            await _service.DeleteCart(cartId);
            return Ok(new { message = "Cart item deleted successfully" });
        }

        // PUT: api/cart/update-quantities
        [HttpPut("update-quantities")]
        public async Task<ActionResult> UpdateQuantities([FromBody] List<CartQuantityUpdateDTO> updates)
        {
            await _service.UpdateQuantity(updates);
            return Ok(new { message = "Cart quantities updated successfully" });
        }
        [HttpGet("{userId}/count")]
        // [Authorize(Roles = "Customer")] // Thêm Authorize nếu cần
        public async Task<ActionResult<int>> GetCartCount(int userId)
        {
            var count = await _service.GetCartCountByUserIdAsync(userId);
            return Ok(count);
        }
    }
}
