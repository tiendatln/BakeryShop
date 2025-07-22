namespace OrderAPI.DTOs
{
    public class UpdateOrderDTO
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public DateTime OrderDate { get; set; } 

        public decimal TotalAmount { get; set; }

        public string ShippingAddress { get; set; }

        public string OrderStatus { get; set; } = "Pending";

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; } = "Pending";
    }
}
