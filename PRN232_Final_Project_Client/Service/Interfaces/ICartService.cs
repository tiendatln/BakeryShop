using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartAPI.DTOs;

namespace Service.Interfaces
{
    public interface ICartService
    {

        Task<List<CartDTO>> GetCartAsync(string token);
        Task<bool> AddCartAsync(CartCreateDTO dto, string token);
        Task<bool> DeleteCartAsync(int cartId, string token);
        Task<bool> UpdateQuantitiesAsync(List<CartQuantityUpdateDTO> updates, string token);
    }
}
