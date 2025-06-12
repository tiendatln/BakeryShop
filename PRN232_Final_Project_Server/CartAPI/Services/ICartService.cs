using CartAPI.DTOs;

namespace CartAPI.Services
{
    public interface ICartService
    {
        IQueryable<CartDTO> GetCartQueryableByUserId(int userId);
        Task AddCart(CartCreateDTO dto);
        Task DeleteCart(int cartID);
        Task UpdateQuantity(List<CartQuantityUpdateDTO> updates);
    }
}
