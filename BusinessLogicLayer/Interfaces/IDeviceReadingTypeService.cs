using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDeviceReadingTypeService
    {
        Task<IEnumerable<DeviceReadingTypeDto>> GetAllDeviceReadingTypesAsync();
        Task<bool> AddDeviceReadingTypeAsync(DeviceReadingTypeDto deviceReadingTypeDto);
        Task<ValidationResult> UpdateDeviceReadingTypeAsync(DeviceReadingTypeDto deviceReadingTypeDto);
        Task<bool> DeleteDeviceReadingTypeByIdAsync(int deviceReadingTypeId);
        Task<bool> IsDeviceReadingTypeUsedAsync(int deviceReadingTypeId);
        DeviceReadingTypeDto ConvertDeviceReadingTypeToDto(DeviceReadingType deviceReadingType);
    }
}