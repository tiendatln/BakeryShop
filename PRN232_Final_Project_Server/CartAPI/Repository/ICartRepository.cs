using CartAPI.Models;

namespace CartAPI.Repositories
{
    public interface ICartRepository
    {
        IQueryable<Cart> GetCartByUserIDQueryable(int userId);
        Task AddCart(Cart cart);
        Task DeleteCart(int cartID);
        Task UpdateQuantity(Cart updateCart);
        Task<int> GetCartCountByUserIdAsync(int userId);
    }
}
