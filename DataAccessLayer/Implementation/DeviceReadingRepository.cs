using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class DeviceReadingRepository : IDeviceReadingRepository
    {
        private readonly InternshipContext _internshipContext;

        public DeviceReadingRepository(InternshipContext internshipContext)
        {
            _internshipContext = internshipContext;
        }

        public bool IsReadingTypeFoundInDevice(string readingTypeName)
        {
            return _internshipContext.DeviceReadings.Any(deviceReading => deviceReading.DeviceReadingType.Name == readingTypeName);
        }

        public async Task AddDeviceReadingAsync(DeviceReading deviceReading)
        {
            await _internshipContext.DeviceReadings.AddAsync(deviceReading);
            await _internshipContext.SaveChangesAsync();
        }

        public async Task<DeviceReadingType> GetReadingTypeByName(string readingTypeName)
        {
            var readingType = await _internshipContext.DeviceReadingTypes
               .FirstOrDefaultAsync(d => d.Name.Equals(readingTypeName));
            return readingType;
        }

        public async Task MarkAlertAsReadAsync(int[] ids)
        {
            var existingDeviceReadingInDb = await _internshipContext.DeviceReadings
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();

            foreach (var deviceReadingMarked in existingDeviceReadingInDb)
            {
                deviceReadingMarked.IsAlertRead = true;
            }

            await _internshipContext.SaveChangesAsync();
        }

        public async Task<int> GetUnreadAlertsCountAsync()
        {
            var unreadAlerts = await _internshipContext.DeviceReadings.Where(d => !d.IsAlertRead && d.AlertType != null).CountAsync();

            return unreadAlerts;
        }
    }
}