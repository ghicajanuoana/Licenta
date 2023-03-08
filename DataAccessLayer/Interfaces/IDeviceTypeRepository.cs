using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IDeviceTypeRepository
    {
        Task AddDeviceTypeAsync(DeviceType deviceType);

        Task UpdateDeviceTypeAsync(DeviceType deviceType);

        Task<IEnumerable<DeviceType>> GetAllDeviceTypesAsync();

        Task<DeviceType> GetDeviceTypeByNameAsync(string deviceName);

        Task DeleteDeviceTypeByIdAsync(int deviceTypeId);

        Task<bool> ValidateUniqueNameAsync(string deviceName);

        Task<bool> DeviceTypeIdExistsAsync(int deviceTypeId);

        Task<DeviceType> GetDeviceTypeByIdAsync(int deviceTypeId);
    }
}