namespace CartAPI.DTOs
{
    public class CartCreateDTO
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

    }
}
