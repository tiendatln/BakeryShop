using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
namespace OrderAPI.Data
{
    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions<OrderDBContext> options)
            : base(options) { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình quan hệ
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình Identity để bắt đầu từ ID mong muốn
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderID)
                .UseIdentityColumn(); // Bắt đầu từ 1, tăng 1

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.OrderDetailID)
                .UseIdentityColumn(); // Bắt đầu từ 1, tăng 1

            // ===== Seed Data =====
            var rand = new Random(123);
            var orders = new List<Order>();
            for (int i = 0; i < 20; i++)
            {
                orders.Add(new Order
                {
                    OrderID = i + 1, // ID từ 1 đến 20
                    UserID = 2,
                    OrderDate = new DateTime(2025, 6, 1).AddDays(i),
                    TotalAmount = 100 + i * 50,
                    ShippingAddress = $"Quận {rand.Next(1, 13)} - TP.HCM",
                    OrderStatus = i % 2 == 0 ? "Pending" : "Shipped",
                    PaymentMethod = i % 2 == 0 ? "Credit Card" : "COD",
                    PaymentStatus = i % 3 == 0 ? "Paid" : "Pending"
                });
            }
            modelBuilder.Entity<Order>().HasData(orders);

            var orderDetails = new List<OrderDetail>();
            int detailId = 1; // Bắt đầu từ 1
            for (int i = 0; i < 20; i++) 
            {
                int numDetails = rand.Next(1, 4); // 1-3 chi tiết mỗi đơn
                for (int j = 0; j < numDetails; j++)
                {
                    orderDetails.Add(new OrderDetail
                    {
                        OrderDetailID = detailId++, // Tăng dần từ 1
                        OrderID = i + 1, // OrderID từ 1 đến 10
                        ProductID = rand.Next(1, 30),
                        Quantity = rand.Next(1, 5),
                        UnitPrice = rand.Next(10, 100)
                    });
                }
            }
            modelBuilder.Entity<OrderDetail>().HasData(orderDetails);
            base.OnModelCreating(modelBuilder);
        }
    }
}