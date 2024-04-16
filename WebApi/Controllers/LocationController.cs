using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationController> _logger;

        CommonStrings common = new CommonStrings();

        public LocationController(ILocationService locationService, ILogger<LocationController> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }

        [HttpPost]
        [Route("addLocation")]
        public async Task<IActionResult> AddLocationAsync([FromBody] LocationAddDto locationDto)
        {
            if (locationDto == null)
            {
                _logger.LogError(common.NullLocation);
                return BadRequest(common.NullLocation);
            }

            if (string.IsNullOrEmpty(locationDto.Name))
            {
                _logger.LogError(common.InvalidName);
                return BadRequest(common.InvalidName);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(locationDto, Formatting.Indented);
                _logger.LogInformation($"AddLocation for {serializedContent}");
                var isLocationAdded = await _locationService.AddLocationAsync(locationDto);

                if (isLocationAdded)
                {
                    return Ok(locationDto);
                }
                _logger.LogError(common.EnterUniqueName);
                return this.StatusCode(SpecialCodes.Status700HasUniqueName, common.EnterUniqueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("updateLocation")]
        public async Task<IActionResult> UpdateLocationAsync([FromBody] LocationAddDto locationDto)
        {
            if (locationDto == null)
            {
                _logger.LogError(common.NullLocation);
                return BadRequest(common.NullLocation);
            }

            if (string.IsNullOrEmpty(locationDto.Name))
            {
                _logger.LogError(common.InvalidName);
                return BadRequest(common.InvalidName);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(locationDto, Formatting.Indented);
                _logger.LogInformation($"UpdateLocation for {serializedContent}");
                var taskResponse = await _locationService.UpdateLocationAsync(locationDto);

                if (taskResponse == ValidationResult.Null)
                {
                    _logger.LogError(common.NullLocation);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundLocation);
                }
                else if (taskResponse == ValidationResult.NotUnique)
                {
                    _logger.LogError(common.NullLocation);
                    return this.StatusCode(SpecialCodes.Status700HasUniqueName, common.EnterUniqueName);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
        /*
        [HttpGet]
        [Route("{locationId}")]
        public async Task<IActionResult> GetLocationByIdAsync(int locationId)
        {
            if (locationId == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"GetLocationById by location id: {locationId}");
                var location = await _locationService.GetLocationByIdAsync(locationId);

                if (location == null)
                {
                    _logger.LogError(common.NullLocation);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundLocation);
                }

                return Ok(location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
        */

        [HttpGet]
        [Route("getAllLocations")]
        public async Task<IActionResult> GetAllLocationsAsync()
        {
            try
            {
                _logger.LogInformation("GetAllLocations");
                var locationDtos = await _locationService.GetAllLocationsAsync();

                return Ok(locationDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{locationId}")]
        public async Task<IActionResult> DeleteLocationByIdAsync(int locationId)
        {
            if (locationId == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"DeleteLocationByIdAsync by location id: {locationId}");
                await _locationService.DeleteLocationByIdAsync(locationId);
                return Ok(common.RemovedLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getLocationsPagedAndFiltered")]
        public async Task<IActionResult> GetLocationsFilteredPagedAsync([FromQuery] LocationParameters locationParameters)
        {
            if (locationParameters == null)
            {
                _logger.LogError(common.NullLocationParameters);
                return BadRequest(common.NullLocationParameters);
            }

            try
            {

                var serializedContent = JsonConvert.SerializeObject(locationParameters, Formatting.Indented);
                _logger.LogInformation($"GetLocationsFilteredPaged for {serializedContent}");
                var locations = await _locationService.GetLocationsFilteredPagedAsync(locationParameters);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("checkLocation/{locationId}")]
        public async Task<IActionResult> CheckLocationIsUsedAsync(int locationId)
        {
            if (locationId == 0)
            {
                _logger.LogError(common.NullDeviceType);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"CheckDeviceTypeIsUsedAsync for device type id : {locationId}");
                await _locationService.CheckLocationIsUsedAsync(locationId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

    }
}