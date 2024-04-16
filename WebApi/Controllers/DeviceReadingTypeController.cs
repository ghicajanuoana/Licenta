using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class DeviceReadingTypeController : ControllerBase
    {
        private readonly IDeviceReadingTypeService _deviceReadingTypeService;
        private readonly ILogger<DeviceReadingTypeController> _logger;

        CommonStrings common = new CommonStrings();

        public DeviceReadingTypeController(IDeviceReadingTypeService deviceReadingTypeService, ILogger<DeviceReadingTypeController> logger)
        {
            this._deviceReadingTypeService = deviceReadingTypeService;
            _logger = logger;
        }

        [HttpPost]
        [Route("addDeviceReadingType")]
        public async Task<IActionResult> AddDeviceReadingTypeAsync([FromBody] DeviceReadingTypeDto deviceReadingTypeDto)
        {
            if (deviceReadingTypeDto == null)
            {
                _logger.LogError(common.NullDeviceReadingType);
                return BadRequest(common.NullDeviceReadingType);
            }

            if (string.IsNullOrEmpty(deviceReadingTypeDto.Name))
            {
                _logger.LogError(common.InvalidName);
                return BadRequest(common.InvalidName);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(deviceReadingTypeDto, Formatting.Indented);
                _logger.LogInformation($"AddDeviceReadingType for {serializedContent}");
                var isDeviceReadingTypeAdded = await _deviceReadingTypeService.AddDeviceReadingTypeAsync(deviceReadingTypeDto);

                if (isDeviceReadingTypeAdded)
                {
                    return Ok(deviceReadingTypeDto);
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
        [Route("updateDeviceReadingType")]
        public async Task<IActionResult> UpdateDeviceReadingTypeAsync([FromBody] DeviceReadingTypeDto deviceReadingTypeDto)
        {
            if (deviceReadingTypeDto == null)
            {
                _logger.LogError(common.NullDeviceReadingType);
                return BadRequest(common.NullDeviceReadingType);
            }

            if (string.IsNullOrEmpty(deviceReadingTypeDto.Name))
            {
                _logger.LogError(common.InvalidName);
                return BadRequest(common.InvalidName);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(deviceReadingTypeDto, Formatting.Indented);
                _logger.LogInformation($"UpdateDeviceReadingType for {serializedContent}");
                var response = await _deviceReadingTypeService.UpdateDeviceReadingTypeAsync(deviceReadingTypeDto);

                if (response == ValidationResult.Null)
                {
                    _logger.LogError(common.NotFoundDeviceReadingType);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundDeviceReadingType);
                }
                else if (response == ValidationResult.NotUnique)
                {
                    _logger.LogError(common.EnterUniqueName);
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

        [HttpGet]
        [Route("getAllDeviceReadingTypes")]
        public async Task<IActionResult> GetAllDeviceReadingTypes()
        {
            try
            {
                _logger.LogInformation($"GetAllDeviceReadingTypes");
                var deviceReadingTypesDtos = await _deviceReadingTypeService.GetAllDeviceReadingTypesAsync();

                return Ok(deviceReadingTypesDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{deviceReadingTypeId}")]
        public async Task<IActionResult> DeleteDeviceReadingTypeByIdAsync(int deviceReadingTypeId)
        {
            if (deviceReadingTypeId == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"DeleteDeviceReadingTypeById by device reading id: {deviceReadingTypeId}");
                var isDeviceReadingTypeUsed = await _deviceReadingTypeService.DeleteDeviceReadingTypeByIdAsync(deviceReadingTypeId);

                if (!isDeviceReadingTypeUsed)
                {
                    _logger.LogInformation(common.RemovedDeviceReadingType);
                    return Ok(common.RemovedDeviceReadingType);
                }
                else
                {
                    _logger.LogError(common.UsedDeviceReadingType);
                    return this.StatusCode(SpecialCodes.Status701IsUsed, common.UsedDeviceReadingType);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet("checkDeviceReadingType/{deviceReadingTypeId}")]
        public async Task<IActionResult> CheckDeviceReadingTypeIsUsedAsync(int deviceReadingTypeId)
        {
            if (deviceReadingTypeId == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"CheckDeviceReadingTypeIsUsed for device reading type id: {deviceReadingTypeId}");
                bool isDeviceReadingTypeUsed = await _deviceReadingTypeService.IsDeviceReadingTypeUsedAsync(deviceReadingTypeId);
                return Ok(isDeviceReadingTypeUsed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
