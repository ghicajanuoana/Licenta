using DataAccessLayer.Models; 

namespace DataAccessLayer.Interfaces
{
    public interface IDeviceReadingRepository
    {
        Task AddDeviceReadingAsync(DeviceReading deviceReading);

        bool IsReadingTypeFoundInDevice(string readingTypeName);

        Task<DeviceReadingType> GetReadingTypeByName(string readingTypeName);

        Task MarkAlertAsReadAsync(int[] ids);

        Task<int> GetUnreadAlertsCountAsync();
    }
}
