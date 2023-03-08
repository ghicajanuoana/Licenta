using BusinessLogicLayer.DTOs;
using Common;
using DataAccessLayer.FilterModels;
using ValidationResult = BusinessLogicLayer.Enums.ValidationResult;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDeviceMaintenanceService
    {
        Task<bool> AddDeviceMaintenanceAsync(MaintenanceAddDto maintenanceDto);
        Task DeleteDeviceMaintenanceByIdAsync(int deviceMaintenanceId);
        Task<MaintenanceDto> GetDeviceMaintenanceByIdAsync(int deviceMaintenanceId);
        Task<ValidationResult> UpdateDeviceMaintenanceAsync(MaintenanceUpdateDto maintenanceDto);
        Task<PagedResponse<MaintenanceDto>> GetDeviceMaintenancesPagedAndSortedAsync(MaintenanceFilter maintenanceFilter);
    }
}
