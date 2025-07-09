namespace FeedbackAPI.DTOs
{
    public class ReadFeedbackDTO
    {
        public int FeedbackID { get; set; }

        public int UserID { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime SubmittedDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
