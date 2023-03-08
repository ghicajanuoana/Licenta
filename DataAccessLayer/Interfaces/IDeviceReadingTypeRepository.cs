using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IDeviceReadingTypeRepository
    {
        Task AddDeviceReadingTypeAsync(DeviceReadingType deviceReadingType);
        bool IsDeviceReadingTypeNameUnique(int id, string name);
        Task<DeviceReadingType> GetDeviceReadingTypeByIdAsync(int deviceReadingTypeId);
        Task UpdateDeviceReadingTypeAsync(DeviceReadingType deviceReadingType);
        Task<IEnumerable<DeviceReadingType>> GetAllDeviceReadingTypesAsync();
        Task DeleteDeviceReadingTypeByIdAsync(int deviceReadingTypeId);
        Task<bool> DeviceReadingTypeIdExistsAsync(int deviceReadingTypeId);
    }
}
