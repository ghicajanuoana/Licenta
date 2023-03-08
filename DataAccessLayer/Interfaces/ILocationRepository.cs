using Common;
using DataAccessLayer.Models;
using static DataAccessLayer.Implementation.LocationRepository;

namespace DataAccessLayer.Interfaces
{
    public interface ILocationRepository
    {
        Task AddLocationAsync(Location location);
        bool IsLocationNameUnique(int id, string name);
        Task<Location> GetLocationByIdAsync(int locationId);
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationByIdAsync(int locationId);
        Task<PagedResponse<Location>> GetLocationsFilteredPagedAsync(PagingFilteringParameters pagingFilteringParameters, Location location);
    }
}
