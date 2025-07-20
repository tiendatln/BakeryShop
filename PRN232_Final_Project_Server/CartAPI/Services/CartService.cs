using AutoMapper;
using CartAPI.DTOs;
using CartAPI.Models;
using CartAPI.Repositories;
using CartAPI.Services;

namespace CartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task AddCart(CartCreateDTO dto)
        {
            var cart = _mapper.Map<Cart>(dto);
            cart.LastUpdated = DateTime.UtcNow;
            return _repository.AddCart(cart);
        }

        public async Task DeleteCart(int cartID)
        {
            await _repository.DeleteCart(cartID);
        }

        public IQueryable<CartDTO> GetCartQueryableByUserId(int userId)
        {
            return _mapper.ProjectTo<CartDTO>(_repository.GetCartByUserIDQueryable(userId));
        }

        public async Task UpdateQuantity(List<CartQuantityUpdateDTO> updates)
        {
            foreach (var update in updates)
            {
                var cart = _mapper.Map<Cart>(update);
                await _repository.UpdateQuantity(cart);
            }
        }

        public async Task<int> GetCartCountByUserIdAsync(int userId)
        {
            return await _repository.GetCartCountByUserIdAsync(userId);
        }
    }
}