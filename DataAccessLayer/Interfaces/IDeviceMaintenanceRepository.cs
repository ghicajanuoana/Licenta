using DataAccessLayer.FilterModels;
using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Interfaces
{
    public interface IDeviceMaintenanceRepository
    {
        Task AddDeviceMaintenanceAsync(Maintenance maintenance);
        bool IsDeviceUsed(int deviceId);
        Task DeleteDeviceMaintenanceByIdAsync(int maintenanceId);
        Task<Maintenance> GetDeviceMaintenanceByIdAsync(int deviceMaintenanceId);
        Task UpdateDeviceMaintenanceAsync(Maintenance maintenance);
        Task<IEnumerable<Maintenance>> GetDeviceMaintenanceOrderedAndFilteredAsync(MaintenanceFilter maintenanceFilter);
    }
}
