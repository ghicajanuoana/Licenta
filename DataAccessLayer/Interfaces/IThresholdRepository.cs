using DataAccessLayer.Models; 

namespace DataAccessLayer.Interfaces
{
    public interface IThresholdRepository
    {
        Task<IEnumerable<Threshold>> GetAllThresholdsAsync();
        Task<bool> IsDeviceReadingTypeUsedAsync(int deviceReadingTypeId);
        Task AddThresholdAsync(Threshold threshold);
        Task DeleteThresholdByIdAsync(int thresholdId);
        Task<bool> DevicesIdAlreadyExistsInThresholdAsync(int deviceTypeId, int deviceReadingTypeId);
        Task UpdateThresholdAsync(Threshold threshold);
        Task<bool> IsFoundDuplicateThreshold(int deviceTypeId, int deviceReadingId, int thresholdId);
        Task<Threshold> GetThresholdByIdAsync(int thresholdId);
    }
}
