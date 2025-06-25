using DTOs.FeedbackDTO;

namespace Service.Interfaces
{

    public interface IFeedbackService
    {
    Task<List<ReadFeedbackDTO>> GetAllAsync(string token);
    Task<ReadFeedbackDTO> GetByIdAsync(int id, string token);
    Task<bool> CreateAsync(CreateFeedbackDTO feedback, string token);
    Task<bool> UpdateAsync(int id, UpdateFeedbackDTO feedback, string token);
    Task<bool> DeleteAsync(int id, string token);
    }
}