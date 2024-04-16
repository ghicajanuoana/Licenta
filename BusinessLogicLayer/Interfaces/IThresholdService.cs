using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums; 

namespace BusinessLogicLayer.Interfaces
{
    public interface IThresholdService
    {
        Task<IEnumerable<ThresholdDto>> GetAllThresholdsAsync();
        Task<bool> AddThresholdAsync(ThresholdDto newThreshold);
        Task<ValidationResult> UpdateThresholdAsync(ThresholdDto thresholdDto);
        Task DeleteThresholdByIdAsync(int thresholdId);
        Task<ThresholdDto> GetThresholdByIdAsync(int thresholdId);
    }
}
