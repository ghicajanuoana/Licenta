using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class DeviceTypeRepository : IDeviceTypeRepository
    {
        public readonly DataContext dataContext;

        public DeviceTypeRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task AddDeviceTypeAsync(DeviceType deviceType)
        {
            if (deviceType == null)
            {
                return;
            }

            await dataContext.DeviceTypes.AddAsync(deviceType);
            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> ValidateUniqueNameAsync(string deviceName)
        {
            return await dataContext.DeviceTypes.AnyAsync(d => d.Name == deviceName) ? false : true;
        }

        public async Task<bool> DeviceTypeIdExistsAsync(int deviceTypeId)
        {
            return await dataContext.DeviceTypes.AnyAsync(d => d.Id == deviceTypeId);
        }

        public async Task UpdateDeviceTypeAsync(DeviceType deviceType)
        {
            var existingDeviceTypeInDb = await GetDeviceTypeByIdAsync(deviceType.Id);

            existingDeviceTypeInDb.Name = deviceType.Name;
            //existingDeviceTypeInDb.Unit = deviceType.Unit;

            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteDeviceTypeByIdAsync(int deviceTypeId)
        {
            var existingDeviceTypeInDb = await GetDeviceTypeByIdAsync(deviceTypeId);

            dataContext.DeviceTypes.Remove(existingDeviceTypeInDb);
            await dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeviceType>> GetAllDeviceTypesAsync()
        {
            return await dataContext.DeviceTypes.OrderBy(d => d.Name).ToListAsync();
        }

        public async Task<DeviceType> GetDeviceTypeByNameAsync(string deviceName)
        {
            var deviceType = await dataContext.DeviceTypes
                .Where(d => d.Name.Equals(deviceName))
                .FirstOrDefaultAsync();

            return deviceType;
        }

        public async Task<DeviceType> GetDeviceTypeByIdAsync(int deviceTypeId)
        {
            var deviceType = await dataContext.DeviceTypes
                .Where(d => d.Id == deviceTypeId)
                .FirstOrDefaultAsync();

            return deviceType;
        }
    }
}
