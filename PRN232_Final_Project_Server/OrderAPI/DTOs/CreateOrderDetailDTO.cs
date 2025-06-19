namespace OrderAPI.DTOs
{
    public class CreateOrderDetailDTO
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
