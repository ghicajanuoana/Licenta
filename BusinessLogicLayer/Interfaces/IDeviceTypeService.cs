using BusinessLogicLayer.DTOs; 
using BusinessLogicLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDeviceTypeService
    {
        Task<IEnumerable<DeviceTypeDto>> GetAllDeviceTypesAsync();

        Task<DeviceTypeDto> GetDeviceTypeByNameAsync(string deviceTypeName);

        Task<bool> AddDeviceTypeAsync(DeviceTypeDto deviceTypeDto);

        Task<ValidationResult> UpdateDeviceTypeAsync(DeviceTypeDto deviceTypeDto);

        Task DeleteDeviceTypeByIdAsync(int deviceTypeId);

        Task CheckDeviceTypeIsUsedAsync(int deviceTypeId);
        DeviceTypeDto ConvertDeviceTypeToDto(DeviceType deviceType);
    }
}
