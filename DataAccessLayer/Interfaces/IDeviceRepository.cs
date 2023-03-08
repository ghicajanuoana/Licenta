using Common;
using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IDeviceRepository
    {
        Task AddDeviceAsync(Device newDevice);
        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<IEnumerable<Device>> GetAllDevicesByLocationIdAsync(int locationId);
        Task<IEnumerable<Device>> GetDevicesByNameAsync(string deviceName);
        Task<Device> GetDeviceByIdAsync(int deviceId);
        bool IsDeviceTypeUsed(int deviceTypeId);
        Task DeleteDeviceByIdAsync(int deviceId);
        bool IsLocationUsed(int locationId);
        Task UpdateDeviceAsync(Device device);
        Task<PagedResponse<Device>> GetDevicesFilteredPagedAsync(PagingFilteringParameters pagingFilteringParameters, Device device);
    }
}