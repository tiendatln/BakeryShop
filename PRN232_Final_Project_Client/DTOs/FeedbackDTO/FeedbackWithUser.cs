using System.ComponentModel.DataAnnotations;

namespace DTOs.FeedbackDTO
{
    public class FeedbackWithUser
    {
        public int FeedbackID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime SubmittedDate { get; set; }

        // Thông tin user thêm vào
        public string UserName { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    
}
