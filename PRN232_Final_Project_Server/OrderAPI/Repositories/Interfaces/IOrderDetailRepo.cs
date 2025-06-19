using OrderAPI.Models;

namespace OrderAPI.Repositories.Interfaces
{
    public interface IOrderDetailRepo
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetails();
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderId(int orderId);
        Task<OrderDetail?> GetOrderDetailById(int id);
        Task<OrderDetail> CreateOrderDetail(OrderDetail orderDetail);
        Task<OrderDetail> UpdateOrderDetail(int id, OrderDetail orderDetail);
        Task<bool> DeleteOrderDetail(int id);
    }
}
