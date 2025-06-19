using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Models;
using OrderAPI.Repositories.Interfaces;

namespace OrderAPI.Repositories
{
    public class OrderDetailRepo : IOrderDetailRepo
    {
        private readonly OrderDBContext _context;

        public OrderDetailRepo(OrderDBContext context)
        {
            _context = context;
        }

        // Method to get all order details
        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetails()
        {
            if (_context?.OrderDetails != null)
            {
                return await _context.OrderDetails
                    .Include(od => od.Order) // Include related Order data
                    .ToListAsync();
            }
            return new List<OrderDetail>();
        }

        // Method to get order details by order ID
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderId(int orderId)
        {
            if (_context?.OrderDetails != null)
            {
                return await _context.OrderDetails
                    .Where(od => od.OrderID == orderId)
                    .Include(od => od.Order)
                    .ToListAsync();
            }
            return new List<OrderDetail>();
        }

        // Method to get a specific order detail by ID
        public async Task<OrderDetail?> GetOrderDetailById(int id)
        {
            if (_context?.OrderDetails != null)
            {
                return await _context.OrderDetails
                    .Include(od => od.Order)
                    .FirstOrDefaultAsync(od => od.OrderDetailID == id);
            }
            return null;
        }

        // Method to create a new order detail
        public async Task<OrderDetail> CreateOrderDetail(OrderDetail orderDetail)
        {
            if (_context?.OrderDetails != null)
            {
                await _context.OrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
                return orderDetail;
            }
            throw new InvalidOperationException("Context or OrderDetails DbSet is null.");
        }

        // Method to update an existing order detail
        public async Task<OrderDetail?> UpdateOrderDetail(int id, OrderDetail updatedDetail)
        {
            if (_context == null || _context.OrderDetails == null)
                return null;

            var existingDetail = await _context.OrderDetails.FindAsync(id);
            if (existingDetail == null)
                return null;

            // Update every attribute
            // Avoid entity not able to track
            existingDetail.Quantity = updatedDetail.Quantity;
            existingDetail.UnitPrice = updatedDetail.UnitPrice;

            await _context.SaveChangesAsync();
            return existingDetail;
        }


        // Method to delete an order detail
        public async Task<bool> DeleteOrderDetail(int id)
        {
            if (_context?.OrderDetails != null)
            {
                var orderDetail = await _context.OrderDetails.FindAsync(id);
                if (orderDetail == null)
                {
                    return false; // Not found
                }

                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
                return true; // Successfully deleted
            }
            throw new InvalidOperationException("Context or OrderDetails DbSet is null.");
        }
    }
}
