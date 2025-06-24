using CartAPI.Data;
using CartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CartAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _context;

        public CartRepository(CartDbContext context)
        {
            _context = context;
        }

        public Task AddCart(Cart cart)
        {
            cart.LastUpdated = DateTime.UtcNow;
            _context.Add(cart);
            return _context.SaveChangesAsync();
        }

        public async Task DeleteCart(int cartID)
        {
            var cart = await _context.Carts.FindAsync(cartID);
            if (cart != null)
            {
                _context.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Cart> GetCartByUserIDQueryable(int userId)
        {
            return _context.Carts.Where(c => c.UserID == userId);
        }

        public async Task UpdateQuantity(Cart updateCart)
        {
            var cart = await _context.Carts.FindAsync(updateCart.CartID);
            if(cart != null)
            {
                cart.Quantity = updateCart.Quantity;
                cart.LastUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
