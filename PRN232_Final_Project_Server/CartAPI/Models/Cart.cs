using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartAPI.Models
{
    public class Cart
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int CartID { get; set; }

        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
