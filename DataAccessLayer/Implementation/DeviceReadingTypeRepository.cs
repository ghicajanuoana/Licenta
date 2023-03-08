using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class DeviceReadingTypeRepository : IDeviceReadingTypeRepository
    {
        private readonly InternshipContext _internshipContext;

        public DeviceReadingTypeRepository(InternshipContext internshipContext)
        {
            this._internshipContext = internshipContext;
        }

        public async Task AddDeviceReadingTypeAsync(DeviceReadingType deviceReadingType)
        {
            if (deviceReadingType == null)
            {
                return;
            }
            await _internshipContext.DeviceReadingTypes.AddAsync(deviceReadingType);
            await _internshipContext.SaveChangesAsync();
        }

        public async Task DeleteDeviceReadingTypeByIdAsync(int deviceReadingTypeId)
        {
            var existingDeviceReadingType = await GetDeviceReadingTypeByIdAsync(deviceReadingTypeId);
            _internshipContext.DeviceReadingTypes.Remove(existingDeviceReadingType);
            await _internshipContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeviceReadingType>> GetAllDeviceReadingTypesAsync()
        {
            return await _internshipContext.DeviceReadingTypes.OrderBy(d => d.Name).ToListAsync();
        }

        public bool IsDeviceReadingTypeNameUnique(int id, string name)
        {
            return !_internshipContext.DeviceReadingTypes.Any(deviceReadingType => deviceReadingType.Id != id && deviceReadingType.Name == name);
        }

        public async Task<DeviceReadingType> GetDeviceReadingTypeByIdAsync(int deviceReadingTypeId)
        {
            var deviceReadingType = await _internshipContext.DeviceReadingTypes
                .FirstOrDefaultAsync(d => d.Id == deviceReadingTypeId);

            return deviceReadingType;
        }

        public async Task UpdateDeviceReadingTypeAsync(DeviceReadingType deviceReadingType)
        {
            var existingDeviceReadingType = await GetDeviceReadingTypeByIdAsync(deviceReadingType.Id);

            if (existingDeviceReadingType != null)
            {
                existingDeviceReadingType.Name = deviceReadingType.Name;
                existingDeviceReadingType.Unit = deviceReadingType.Unit;
                await _internshipContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException(nameof(deviceReadingType));
            }
        }

        public async Task<bool> DeviceReadingTypeIdExistsAsync(int deviceReadingTypeId)
        {
            return await _internshipContext.DeviceReadingTypes.AnyAsync(d => d.Id == deviceReadingTypeId);
        }
    }
}