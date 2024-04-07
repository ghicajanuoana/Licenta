using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class ThresholdRepository : IThresholdRepository
    {
        private readonly DataContext _dataContext;

        public ThresholdRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Threshold>> GetAllThresholdsAsync()
        {
            var thresholds = await _dataContext.Thresholds
                .Include(d => d.DeviceType)
                .Include(d => d.DeviceReadingType)
                .OrderBy(d => d.DeviceType.Name)
                .ToListAsync();
            return thresholds;
        }

        public async Task<bool> IsDeviceReadingTypeUsedAsync(int deviceReadingTypeId)
        {
            var doesDeviceReadingTypeExist = _dataContext.Thresholds.AnyAsync(d => d.DeviceReadingType.Id == deviceReadingTypeId);
            return await doesDeviceReadingTypeExist ? true : false;
        }

        public async Task<bool> DevicesIdAlreadyExistsInThresholdAsync(int deviceTypeId, int deviceReadingId)
        {
            return await _dataContext.Thresholds.AnyAsync(d => d.DeviceTypeId == deviceTypeId && d.DeviceReadingTypeId == deviceReadingId);
        }

        public async Task<bool> IsFoundDuplicateThreshold(int deviceTypeId, int deviceReadingId, int thresholdId)
        {
            return await _dataContext.Thresholds.AnyAsync(d => d.DeviceTypeId == deviceTypeId && d.DeviceReadingTypeId == deviceReadingId && thresholdId != d.Id);
        }

        public async Task AddThresholdAsync(Threshold threshold)
        {
            await _dataContext.Thresholds.AddAsync(threshold);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateThresholdAsync(Threshold threshold)
        {
            _dataContext.Thresholds.Update(threshold);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteThresholdByIdAsync(int thresholdId)
        {
            var existingThreshold = await GetThresholdByIdAsync(thresholdId);

            _dataContext.Thresholds.Remove(existingThreshold);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Threshold> GetThresholdByIdAsync(int thresholdId)
        {
            var threshold = await _dataContext.Thresholds
                .Include(d => d.DeviceType)
                .Include(d => d.DeviceReadingType)
                .FirstOrDefaultAsync(d => d.Id == thresholdId);

            return threshold;
        }
    }
}