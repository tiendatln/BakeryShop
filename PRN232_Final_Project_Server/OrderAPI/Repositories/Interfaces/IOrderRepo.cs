using OrderAPI.Models;

namespace OrderAPI.Repositories.Interfaces
{
    public interface IOrderRepo
    {
        Task<IEnumerable<Order>> GetAllOrder();
        IQueryable<Order> GetAllOrderQueryable();
        Task<List<Order>> GetOrderByUserId(int id);
        IQueryable<Order> GetOrderByUserIdQueryable(int id);
        Task<int> GetOrderCountByUserIdAsync(int userId);
        Task<Order> CreateOrder(Order order);
        Task<Order> UpdateOrder(int id, Order order);
        Task<bool> DeleteOrders(int id);
    }
}
