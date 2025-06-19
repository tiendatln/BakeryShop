using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(255)]
        public string ShippingAddress { get; set; }

        [Required]
        [MaxLength(20)]
        public string OrderStatus { get; set; } = "Pending";

        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending";

        //get list of order details
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
