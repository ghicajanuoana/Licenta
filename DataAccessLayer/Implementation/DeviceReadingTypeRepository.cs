using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class DeviceReadingTypeRepository : IDeviceReadingTypeRepository
    {
        private readonly DataContext _dataContext;

        public DeviceReadingTypeRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task AddDeviceReadingTypeAsync(DeviceReadingType deviceReadingType)
        {
            if (deviceReadingType == null)
            {
                return;
            }
            await _dataContext.DeviceReadingTypes.AddAsync(deviceReadingType);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteDeviceReadingTypeByIdAsync(int deviceReadingTypeId)
        {
            var existingDeviceReadingType = await GetDeviceReadingTypeByIdAsync(deviceReadingTypeId);
            _dataContext.DeviceReadingTypes.Remove(existingDeviceReadingType);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeviceReadingType>> GetAllDeviceReadingTypesAsync()
        {
            return await _dataContext.DeviceReadingTypes.OrderBy(d => d.Name).ToListAsync();
        }

        public bool IsDeviceReadingTypeNameUnique(int id, string name)
        {
            return !_dataContext.DeviceReadingTypes.Any(deviceReadingType => deviceReadingType.Id != id && deviceReadingType.Name == name);
        }

        public async Task<DeviceReadingType> GetDeviceReadingTypeByIdAsync(int deviceReadingTypeId)
        {
            var deviceReadingType = await _dataContext.DeviceReadingTypes
                .FirstOrDefaultAsync(d => d.Id == deviceReadingTypeId);

            return deviceReadingType;
        }

        public async Task UpdateDeviceReadingTypeAsync(DeviceReadingType deviceReadingType)
        {
            var existingDeviceReadingType = await GetDeviceReadingTypeByIdAsync(deviceReadingType.Id);

            if (existingDeviceReadingType != null)
            {
                existingDeviceReadingType.Name = deviceReadingType.Name;
                
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException(nameof(deviceReadingType));
            }
        }

        public async Task<bool> DeviceReadingTypeIdExistsAsync(int deviceReadingTypeId)
        {
            return await _dataContext.DeviceReadingTypes.AnyAsync(d => d.Id == deviceReadingTypeId);
        }
    }
}