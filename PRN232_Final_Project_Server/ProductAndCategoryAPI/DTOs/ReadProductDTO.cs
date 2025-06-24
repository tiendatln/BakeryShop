namespace ProductAndCategoryAPI.DTOs
{
    public class ReadProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryID { get; set; }
        public DateTime CreatedDate { get; set; } = default;
        public string ImageURL { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
    }
}
