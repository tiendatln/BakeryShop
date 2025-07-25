﻿using System.ComponentModel.DataAnnotations;

namespace CartAPI.DTOs
{
    public class CartDTO
    {
        [Key]
        public int CartID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; } 
    }
}
