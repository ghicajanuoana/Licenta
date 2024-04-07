using DataAccessLayer.FilterModels;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static System.Formats.Asn1.AsnWriter;

namespace DataAccessLayer.Implementation
{
    public class DeviceMaintenanceRepository : IDeviceMaintenanceRepository
    {
        private readonly DataContext _dataContext;

        public DeviceMaintenanceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddDeviceMaintenanceAsync(Maintenance maintenance)
        {
            await _dataContext.Maintenances.AddAsync(maintenance);
            await _dataContext.SaveChangesAsync();
        }

        public bool IsDeviceUsed(int deviceId)
        {
            return !_dataContext.Maintenances.Any(d => d.DeviceId == deviceId);
        }

        public async Task<IEnumerable<Maintenance>> GetDeviceMaintenanceOrderedAndFilteredAsync(MaintenanceFilter maintenanceFilter)
        {
            return await _dataContext.Maintenances.Include(m => m.Device)
                .Where(m => maintenanceFilter.Device == null || m.Device.Name.Contains(maintenanceFilter.Device))
                .Where(m => maintenanceFilter.Description == null || m.Description.Contains(maintenanceFilter.Description))
                .Where(m => maintenanceFilter.Outcome == null || m.Outcome.Contains(maintenanceFilter.Outcome))
                .Where(m => maintenanceFilter.Status == null || m.Status == maintenanceFilter.Status)
                .Where(m => maintenanceFilter.CreatedBy == null || m.CreatedBy.Contains(maintenanceFilter.CreatedBy))
                .Where(m => (maintenanceFilter.ScheduledDateStart == null || maintenanceFilter.ScheduledDateEnd == null) || (m.ScheduledDate >= maintenanceFilter.ScheduledDateStart && m.ScheduledDate <= maintenanceFilter.ScheduledDateEnd))
                .Where(m => (maintenanceFilter.ActualDateStart == null || maintenanceFilter.ActualDateEnd == null) || (m.ActualDate >= maintenanceFilter.ActualDateStart && m.ActualDate <= maintenanceFilter.ActualDateEnd))
                .Where(m => (maintenanceFilter.CreatedAtStart == null || maintenanceFilter.CreatedAtEnd == null) || (m.CreatedAt >= maintenanceFilter.CreatedAtStart && m.CreatedAt <= maintenanceFilter.CreatedAtEnd))
                .OrderBy(m => m.Status).ThenBy(m => m.ScheduledDate).ToListAsync();
        }

        public async Task DeleteDeviceMaintenanceByIdAsync(int maintenanceId)
        {
            var existingDeviceMaintenance = await GetDeviceMaintenanceByIdAsync(maintenanceId);

            _dataContext.Maintenances.Remove(existingDeviceMaintenance);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Maintenance> GetDeviceMaintenanceByIdAsync(int deviceMaintenanceId)
        {   
            return await _dataContext.Maintenances.Include(m=>m.Device)
                .FirstOrDefaultAsync(m => m.Id == deviceMaintenanceId);

        }

        public async Task UpdateDeviceMaintenanceAsync(Maintenance maintenance)
        { 
            _dataContext.Maintenances.Update(maintenance);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Maintenance>> GetAllMaintenancesAsync()
        {
            var maint = await _dataContext.Maintenances
                .Include(d => d.Device)
                .ToListAsync();
            return maint;
        }
    }
}
