namespace OrderAPI.DTOs
{
    public class CreateOrderDTO
    {
        public int UserID { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public string ShippingAddress { get; set; }

        public string OrderStatus { get; set; } = "Pending";

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        public List<CreateOrderDetailDTO> OrderDetails { get; set; } = new List<CreateOrderDetailDTO>();
    }
}
