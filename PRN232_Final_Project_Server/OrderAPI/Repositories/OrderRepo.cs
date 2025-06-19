using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Models;
using OrderAPI.Repositories.Interfaces;

namespace OrderAPI.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private OrderDBContext _context;

        public OrderRepo(OrderDBContext context)
        {
            _context = context;
        }

        // Method to get all orders
        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            if (_context != null && _context.Orders != null)
            {
                return await _context.Orders
                    .Include(o => o.OrderDetails) // Include related OrderDetails
                    .ToListAsync();
            }
            return new List<Order>();
        }

        // Method to get an order by ID
        public async Task<Order?> GetOrderById(int id)
        {
            if (_context != null && _context.Orders != null)
            {
                return await _context.Orders
                    .Include(o => o.OrderDetails) // Include related OrderDetails
                    .FirstOrDefaultAsync(o => o.OrderID == id);
            }
            return null;
        }

        //Method to create a new order
        public async Task<Order> CreateOrder(Order order)
        {
            if (_context != null && _context.Orders != null)
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
            }
            return null;
        }

        // Method to update an existing order
        public async Task<Order?> UpdateOrder(int id, Order updatedOrder)
        {
            if (_context == null || _context.Orders == null)
                return null;

            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null)
                return null;

            // Update every attribute
            // Avoid entity not able to track
            existingOrder.UserID = updatedOrder.UserID;
            existingOrder.OrderDate = updatedOrder.OrderDate;
            existingOrder.TotalAmount = updatedOrder.TotalAmount;
            existingOrder.ShippingAddress = updatedOrder.ShippingAddress;
            existingOrder.OrderStatus = updatedOrder.OrderStatus;
            existingOrder.PaymentMethod = updatedOrder.PaymentMethod;
            existingOrder.PaymentStatus = updatedOrder.PaymentStatus;

            await _context.SaveChangesAsync();
            return existingOrder;
        }


        // Method to delete an order by ID
        public async Task<bool> DeleteOrders(int id)
        {
            if (_context != null && _context.Orders != null)
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null) return false;

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
