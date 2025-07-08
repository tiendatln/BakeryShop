using System.ComponentModel.DataAnnotations;

namespace DTOs.FeedbackDTO
{
    public class CreateFeedbackDTO
    {
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}
