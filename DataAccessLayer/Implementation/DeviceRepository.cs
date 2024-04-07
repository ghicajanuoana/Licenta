using Common;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DataContext _dataContext;

        public DeviceRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            var devices = await _dataContext.Devices
                .Include(d => d.Location)
                .Include(d => d.DeviceType)
                .ToListAsync();
            return devices;
        }

        public async Task<IEnumerable<Device>> GetAllDevicesByLocationIdAsync(int locationId)
        {
            var devices = await _dataContext.Devices
                .Include(d => d.Location)
                .Include(d => d.DeviceType)
                .Where(d => d.LocationId == locationId)
                .ToListAsync();
            return devices;
        }

        public async Task AddDeviceAsync(Device newDevice)
        {
            await _dataContext.Devices.AddAsync(newDevice);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByNameAsync(string deviceName)
        {
            var devices = await _dataContext.Devices.Where(d => d.Name.Equals(deviceName)).ToListAsync();
            return devices;
        }

        public bool IsDeviceTypeUsed(int deviceTypeId)
        {
            bool isDeviceTypeInUse = _dataContext.Devices.Any(d => d.DeviceTypeId == deviceTypeId);
            return !isDeviceTypeInUse;
        }

        public async Task DeleteDeviceByIdAsync(int deviceId)
        {
            var existingDevice = await GetDeviceByIdAsync(deviceId);

            _dataContext.Devices.Remove(existingDevice);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Device> GetDeviceByIdAsync(int deviceId)
        {
            var device = await _dataContext.Devices
                .Include(d => d.Location)
                .Include(d => d.DeviceType)
                .FirstOrDefaultAsync(d => d.DeviceId == deviceId);

            return device;
        }

        public bool IsLocationUsed(int locationId)
        {
            return _dataContext.Devices.Any(l => l.LocationId == locationId);
        }

        public async Task UpdateDeviceAsync(Device device)
        {
            _dataContext.Devices.Update(device);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<Device>> GetDevicesFilteredPagedAsync(PagingFilteringParameters pagingFilteringParameters, Device device)
        {
            var filteredDevices = _dataContext.Devices
              .Include(d => d.Location)
              .Include(d => d.DeviceType)
              .Where(d => device.Name == null || d.Name.Contains(device.Name))
              .Where(d => device.SerialNumber == null || d.SerialNumber.Contains(device.SerialNumber))
              .Where(d => device.Description == null || d.Description.Contains(device.Description))
              .Where(d => device.Location == null || d.Location.Name.Contains(device.Location.Name))
              .Where(d => device.DeviceType == null || d.DeviceType.Name.Contains(device.DeviceType.Name));

            var count = filteredDevices.Count();

            IQueryable<Device> orderedDevices = null;

            switch (pagingFilteringParameters.OrderBy)
            {
                case "Name":
                    orderedDevices = pagingFilteringParameters.OrderDescending 
                        ? filteredDevices.OrderByDescending(d => d.Name) 
                        : filteredDevices.OrderBy(d => d.Name);
                    break;
                case "SerialNumber":
                    orderedDevices = pagingFilteringParameters.OrderDescending 
                        ? filteredDevices.OrderByDescending(d => d.SerialNumber) 
                        : filteredDevices.OrderBy(d => d.SerialNumber);
                    break;
                case "Description":
                    orderedDevices = pagingFilteringParameters.OrderDescending 
                        ? filteredDevices.OrderByDescending(d => d.Description) 
                        : filteredDevices.OrderBy(d => d.Description);
                    break;
                case "DeviceType":
                    orderedDevices = pagingFilteringParameters.OrderDescending 
                        ? filteredDevices.OrderByDescending(d => d.DeviceType.Name) 
                        : filteredDevices.OrderBy(d => d.DeviceType.Name);
                    break;
                case "Location":
                    orderedDevices = pagingFilteringParameters.OrderDescending 
                        ? filteredDevices.OrderByDescending(d => d.Location.Name) 
                        : filteredDevices.OrderBy(d => d.Location.Name);
                    break;
            }

            IQueryable<Device> pagedDevices = orderedDevices.Skip((pagingFilteringParameters.PageNumber) * pagingFilteringParameters.PageSize)
             .Take(pagingFilteringParameters.PageSize);

            List<Device> result = await pagedDevices.ToListAsync();

            return new PagedResponse<Device>(result, pagingFilteringParameters.PageNumber,
                pagingFilteringParameters.PageSize, count);
        }
    }
}