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

        // POST: api/cart
        [HttpPost]
        public async Task<ActionResult> AddCart([FromBody] CartCreateDTO dto)
        {

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
    }
}
