using BusinessLogicLayer.DTOs;
using Common;
using DataAccessLayer.Models;
using BusinessLogicLayer.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
        Task<bool> AddDeviceAsync(DeviceDtoAdd newDevice);
        Task DeleteDeviceByIdAsync(int deviceId);
        Task<ValidationResult> UpdateDeviceAsync(DeviceDtoAdd deviceDto);
        Task<DeviceDtoAdd> GetDeviceByIdAsync(int deviceId);
        Task UpdateDevicePingTimestampAsync(DevicePingDto devicePingDto);
        Task<bool> ExportToExcel();
        Task<PagedResponse<DeviceInListDto>> GetDevicesFilteredPagedAsync(DeviceParameters deviceParameters);
        Task<IEnumerable<DeviceDto>> GetAllDevicesByLocationIdAsync(int locationId);
        MaintenanceDeviceDto ConvertDeviceToMaintenanceDeviceDto(Device device);
        Task<MemoryStream> ExportToCSV(DeviceParameters deviceParameters);
    }
}