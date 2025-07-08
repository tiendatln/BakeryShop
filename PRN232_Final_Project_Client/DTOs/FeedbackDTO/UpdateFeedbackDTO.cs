using System.ComponentModel.DataAnnotations;

namespace DTOs.FeedbackDTO
{
    public class UpdateFeedbackDTO
    {
        [Required]
        public int FeedbackID { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}
