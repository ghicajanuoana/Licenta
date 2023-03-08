using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class DeviceTypeRepository : IDeviceTypeRepository
    {
        public readonly InternshipContext internshipContext;

        public DeviceTypeRepository(InternshipContext internshipContext)
        {
            this.internshipContext = internshipContext;
        }

        public async Task AddDeviceTypeAsync(DeviceType deviceType)
        {
            await internshipContext.DeviceTypes.AddAsync(deviceType);
            await internshipContext.SaveChangesAsync();
        }

        public async Task<bool> ValidateUniqueNameAsync(string deviceName)
        {
            return await internshipContext.DeviceTypes.AnyAsync(d => d.Name == deviceName) ? false : true;
        }

        public async Task<bool> DeviceTypeIdExistsAsync(int deviceTypeId)
        {
            return await internshipContext.DeviceTypes.AnyAsync(d => d.Id == deviceTypeId);
        }

        public async Task UpdateDeviceTypeAsync(DeviceType deviceType)
        {
            var existingDeviceTypeInDb = await GetDeviceTypeByIdAsync(deviceType.Id);

            existingDeviceTypeInDb.Name = deviceType.Name;

            await internshipContext.SaveChangesAsync();
        }

        public async Task DeleteDeviceTypeByIdAsync(int deviceTypeId)
        {
            var existingDeviceTypeInDb = await GetDeviceTypeByIdAsync(deviceTypeId);

            internshipContext.DeviceTypes.Remove(existingDeviceTypeInDb);
            await internshipContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeviceType>> GetAllDeviceTypesAsync()
        {
            return await internshipContext.DeviceTypes.OrderBy(d => d.Name).ToListAsync();
        }

        public async Task<DeviceType> GetDeviceTypeByNameAsync(string deviceName)
        {
            var deviceType = await internshipContext.DeviceTypes
                .Where(d => d.Name.Equals(deviceName))
                .FirstOrDefaultAsync();

            return deviceType;
        }

        public async Task<DeviceType> GetDeviceTypeByIdAsync(int deviceTypeId)
        {
            var deviceType = await internshipContext.DeviceTypes
                .Where(d => d.Id == deviceTypeId)
                .FirstOrDefaultAsync();

            return deviceType;
        }
    }
}
