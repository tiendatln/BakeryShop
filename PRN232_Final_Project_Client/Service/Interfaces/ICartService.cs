using DTOs.CartDTO;

// Trong Service/Interfaces/ICartService.cs
namespace Service.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDTO>> GetCartAsync(string token); // Giữ lại nếu vẫn có nơi khác dùng
        Task<List<CartDTO>> GetCartAsync(string token, int skip, int take);
        Task<int> GetCartCountAsync(string token); // <-- THÊM DÒNG NÀY
        Task<bool> AddCartAsync(CartCreateDTO dto, string token);
        Task<bool> DeleteCartAsync(int cartId, string token);
        Task<bool> UpdateQuantitiesAsync(List<CartQuantityUpdateDTO> updates, string token);
    }
}