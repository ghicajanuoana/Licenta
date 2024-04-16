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
    public class DeviceTypesController : ControllerBase 
    {
        private readonly IDeviceTypeService _deviceTypeService;
        private readonly ILogger<DeviceTypesController> _logger;

        CommonStrings common = new CommonStrings();
        private IDeviceTypeService @object;

        public DeviceTypesController(IDeviceTypeService deviceTypeService, ILogger<DeviceTypesController> logger)
        {
            _deviceTypeService = deviceTypeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceTypesAsync()
        {
            try
            {
                _logger.LogInformation("GetAllDeviceTypes");

                var devideTypeDtos = await _deviceTypeService.GetAllDeviceTypesAsync();

                return Ok(devideTypeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{deviceTypeName}")]
        public async Task<IActionResult> GetDeviceTypeByNameAsync(string deviceTypeName)
        {
            if (string.IsNullOrEmpty(deviceTypeName))
            {
                _logger.LogError(common.InvalidName);
                return BadRequest(common.InvalidName);
            }

            try
            {
                _logger.LogInformation($"GetDeviceTypeByName by device type name: {deviceTypeName}");
                var deviceType = await _deviceTypeService.GetDeviceTypeByNameAsync(deviceTypeName);

                if (deviceType == null)
                {
                    _logger.LogError(common.NotFoundDeviceType);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundDeviceType);
                }

                return Ok(deviceType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("addDeviceTypes")]
        public async Task<IActionResult> AddDeviceTypeAsync([FromBody] DeviceTypeDto deviceTypeDto)
        {
            if (deviceTypeDto == null)
            {
                _logger.LogError(common.NullDeviceType);
                return BadRequest(common.NullDeviceType);
            }

            if (string.Equals(deviceTypeDto.Name, "string"))
            {
                _logger.LogError(common.InvalidName);
                return BadRequest(common.InvalidName);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(deviceTypeDto, Formatting.Indented);
                _logger.LogInformation($"AddDeviceType for {serializedContent}");
                var isDeviceTypeAdded = await _deviceTypeService.AddDeviceTypeAsync(deviceTypeDto);

                if (isDeviceTypeAdded)
                {
                    return Ok(deviceTypeDto);
                }
                else
                {
                    _logger.LogError(common.EnterUniqueName);
                    return this.StatusCode(SpecialCodes.Status700HasUniqueName, common.EnterUniqueName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("updateDeviceTypes")]
        public async Task<IActionResult> UpdateDeviceTypeAsync([FromBody] DeviceTypeDto deviceTypeDto)
        {
            if (deviceTypeDto == null)
            {
                _logger.LogError(common.NullDeviceType);
                return BadRequest(common.NullDeviceType);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(deviceTypeDto, Formatting.Indented);
                _logger.LogInformation($"UpdateDeviceType for {serializedContent}");

                var deviceTypeResponse = await _deviceTypeService.UpdateDeviceTypeAsync(deviceTypeDto);
                
                if (deviceTypeResponse == ValidationResult.Null)
                {
                    _logger.LogError(common.NullDeviceType);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundDeviceType);
                }
                else if (deviceTypeResponse == ValidationResult.NotUnique)
                {
                    _logger.LogError(common.NullDeviceType);
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

        [HttpDelete("deleteById/{deviceTypeId}")]
        public async Task<IActionResult> DeleteDeviceTypeByIdAsync(int deviceTypeId)
        {
            if (deviceTypeId == 0)
            {
                _logger.LogError(common.NullDeviceType);
                return BadRequest(common.InvalidIdDeviceType);
            }

            try
            {
                _logger.LogInformation($"DeleteDeviceTypeById by device type id: {deviceTypeId}");
                await _deviceTypeService.DeleteDeviceTypeByIdAsync(deviceTypeId);

                return Ok(common.RemovedDeviceType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet("checkDeviceType/{deviceTypeId}")]
        public async Task<IActionResult> CheckDeviceTypeIsUsedAsync(int deviceTypeId)
        {
            if (deviceTypeId == 0)
            {
                _logger.LogError(common.NullDeviceType);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"CheckDeviceTypeIsUsedAsync for device type id : {deviceTypeId}");
                await _deviceTypeService.CheckDeviceTypeIsUsedAsync(deviceTypeId);

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
