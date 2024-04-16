using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Interfaces;
using Common;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Reflection;
using System.Resources;

namespace BusinessLogicLayer.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        private readonly IDeviceRepository _deviceRepository;

        private readonly IUserService _userService;

        CommonStrings common = new CommonStrings();

        public LocationService(ILocationRepository locationRepository, IDeviceRepository deviceRepository, IUserService userService)
        {
            _locationRepository = locationRepository;
            _deviceRepository = deviceRepository;
            _userService = userService;
        }

        public LocationDto ConvertLocationToDto(Location location)
        {
            var locationDto = new LocationAddDto()
            {
                LocationId = location.LocationId,
                Name = location.Name,
                Address = location.Address,
                City = location.City,
                ContactEmail = location.ContactEmail,
                Country = location.Country,
                EmailAlertsActive = location.EmailAlertsActive,
                EmailRecipient = location.EmailRecipient,
                //User = _userService.ConvertUserToDto(location.User), //ConvertUserToDto(location.User)
                Latitude = location.Latitude,
                Longitude = location.Longitude,
            };
            return locationDto;
        }

        public static Location ConvertDtoToLocation(LocationAddDto locationDto, Location dbLocation = null)
        {
            Location location = dbLocation ?? new Location();
            location.Name = locationDto.Name;
            location.Address = locationDto.Address;
            location.City = locationDto.City;
            location.ContactEmail = locationDto.ContactEmail;
            location.Country = locationDto.Country;
            location.EmailAlertsActive = locationDto.EmailAlertsActive;
            location.EmailRecipient = locationDto.EmailRecipient;
            //location.UserId = locationDto.User.UserId;
            location.Phone = locationDto.Phone;
            location.Latitude = locationDto.Latitude;
            location.Longitude = locationDto.Longitude;

            return location;
        }

        public async Task<bool> AddLocationAsync(LocationAddDto locationDto)
        {
            if (_locationRepository.IsLocationNameUnique(locationDto.LocationId, locationDto.Name))
            {
                var location = ConvertDtoToLocation(locationDto);

                await _locationRepository.AddLocationAsync(location);
                return true;
            }

            return false;
        }

        
        public async Task<LocationAddDto> GetLocationByIdAsync(int locationId)
        {
            var location = await _locationRepository.GetLocationByIdAsync(locationId);

            if (location == null)
            {
                return null;
            }

            var locationResponseDto = new LocationAddDto
            {
                LocationId = location.LocationId,
                Name = location.Name,
                EmailAlertsActive = location.EmailAlertsActive,
                EmailRecipient = location.EmailRecipient,
                ContactEmail = location.ContactEmail,
                Address = location.Address,
                City = location.City,
                Country = location.Country,
                Phone = location.Phone,
                Longitude = location.Longitude,
                Latitude = location.Latitude
            };

            return locationResponseDto;
        }
        

        public async Task<IEnumerable<LocationInListDto>> GetAllLocationsAsync()
        {
            var locations = await _locationRepository.GetAllLocationsAsync();
            var locationDtos = new List<LocationInListDto>();

            foreach (var location in locations)
            {
                var isLocationUsed = _deviceRepository.IsLocationUsed(location.LocationId);
                var locationDto = new LocationInListDto
                {
                    LocationId = location.LocationId,
                    Name = location.Name,
                    Country = location.Country,
                    City = location.City,
                    Address = location.Address,
                    ContactEmail = location.ContactEmail,
                    IsLocationUsed = isLocationUsed
                };
                locationDtos.Add(locationDto);
            }
            return locationDtos;
        }

        public async Task<ValidationResult> UpdateLocationAsync(LocationAddDto locationDto)
        {
            var location = await _locationRepository.GetLocationByIdAsync(locationDto.LocationId);

            if (location == null)
            {
                return ValidationResult.Null;
            }

            else if (_locationRepository.IsLocationNameUnique(locationDto.LocationId, locationDto.Name))
            {
                location = ConvertDtoToLocation(locationDto, location);
                await _locationRepository.UpdateLocationAsync(location);
                return ValidationResult.Success;
            }
            return ValidationResult.NotUnique;
        }

        public async Task DeleteLocationByIdAsync(int locationId)
        {
            if (locationId != null)
            {
                await _locationRepository.DeleteLocationByIdAsync(locationId);
            }
            else
            {
                throw new Exception(common.NotFoundLocation);

            }
        }

        public async Task<PagedResponse<LocationInListDto>> GetLocationsFilteredPagedAsync(LocationParameters locationParameters)
        {
            var location = new Location
            {
                Name = locationParameters.Name,
                Address = locationParameters.Address,
                Country = locationParameters.Country,
                City = locationParameters.City,
                ContactEmail = locationParameters.ContactEmail
            };

            if (locationParameters.PagingFilteringParameters == null)
            {
                locationParameters.PagingFilteringParameters = new PagingFilteringParameters();
            }

            var locations = await _locationRepository.GetLocationsFilteredPagedAsync(locationParameters.PagingFilteringParameters, location);

            var locationDtos = new List<LocationInListDto>();
            foreach (var l in locations.Data)
            {
                var locationsDto = new LocationInListDto
                {
                    LocationId = l.LocationId,
                    Name = l.Name,
                    Country = l.Country,
                    City = l.City,
                    Address = l.Address,
                    ContactEmail = l.ContactEmail
                };
                locationDtos.Add(locationsDto);
            }
            return new PagedResponse<LocationInListDto>(locationDtos, locations.PageNumber,
                locations.PageSize, locations.TotalCount);
        }

        public async Task CheckLocationIsUsedAsync(int locationId)
        {
            if (!_deviceRepository.IsLocationUsed(locationId))
            {
                throw new Exception(common.UsedLocation);
            }
        }
    }
}