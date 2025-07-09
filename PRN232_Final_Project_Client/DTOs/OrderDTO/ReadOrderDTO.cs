using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTO
{
    public class ReadOrderDTO
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public List<ReadOrderDetailDTO> OrderDetails { get; set; } = new List<ReadOrderDetailDTO>();
    }
}
