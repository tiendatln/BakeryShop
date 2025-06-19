using OrderAPI.Models;

namespace OrderAPI.Repositories.Interfaces
{
    public interface IOrderRepo
    {
        Task<IEnumerable<Order>> GetAllOrder();
        Task<Order> GetOrderById(int id);
        Task<Order> CreateOrder(Order order);
        Task<Order> UpdateOrder(int id, Order order);
        Task<bool> DeleteOrders(int id);
    }
}
