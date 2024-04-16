using BusinessLogicLayer.DTOs;
using DataAccessLayer.Models;
using Common;
using BusinessLogicLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface ILocationService
    {
        Task<bool> AddLocationAsync(LocationAddDto locationDto);
        public Task<LocationAddDto> GetLocationByIdAsync(int locationId);
        Task<IEnumerable<LocationInListDto>> GetAllLocationsAsync();
        Task<ValidationResult> UpdateLocationAsync(LocationAddDto locationDto);
        Task DeleteLocationByIdAsync(int locationId);
        Task<PagedResponse<LocationInListDto>> GetLocationsFilteredPagedAsync(LocationParameters locationParameters);
        LocationDto ConvertLocationToDto(Location location);
        Task CheckLocationIsUsedAsync(int locationId);
    }
}