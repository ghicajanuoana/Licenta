using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDeviceReadingService
    {
        Task<bool> AddDeviceReadingAsync(DeviceReadingDto deviceReadingTypeDto);

        Task MarkAlertAsReadAsync(MarkAlertReadDeviceReadingDto markedAlertDto);

        Task<int> GetUnreadAlertsCountAsync();
    }
}
