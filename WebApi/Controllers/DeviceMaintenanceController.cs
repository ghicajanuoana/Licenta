using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.FilterModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BusinessLogicLayer.Enums;
using ValidationResult = BusinessLogicLayer.Enums.ValidationResult;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using BusinessLogicLayer.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceMaintenanceController : ControllerBase
    {
        private readonly IDeviceMaintenanceService _deviceMaintenanceService;
        private readonly ILogger<DeviceMaintenanceController> _logger;

        CommonStrings common = new CommonStrings();

        public DeviceMaintenanceController(IDeviceMaintenanceService deviceMaintenanceService, ILogger<DeviceMaintenanceController> logger)
        {
            _deviceMaintenanceService = deviceMaintenanceService;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddMaintenance")]
        public async Task<IActionResult> AddDeviceMaintenance([FromBody] MaintenanceAddDto maintenance) //[FromBody]
        {
            if (maintenance == null)
            {
                _logger.LogError(common.NullMaintenance);
                return BadRequest(common.NullMaintenance);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(maintenance, Formatting.Indented);
                _logger.LogInformation($"AddDeviceMaintenance for {serializedContent}");
                var isDeviceMaintenanceAdded = await _deviceMaintenanceService.AddDeviceMaintenanceAsync(maintenance);

                if (isDeviceMaintenanceAdded)
                {
                    return Ok(maintenance);
                }

                _logger.LogError(common.UsedDevice);
                return this.StatusCode(SpecialCodes.Status701IsUsed, common.UsedDevice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getDeviceMaintenancesPaged")]
        public async Task<IActionResult> GetDeviceMaintenancesPagedAsync([FromQuery] MaintenanceFilter maintenanceFilter)
        {
            try
            {
                var serializedContent = JsonConvert.SerializeObject(maintenanceFilter, Formatting.Indented);
                _logger.LogInformation($"GetDeviceMaintenancesPaged for {serializedContent}");
                var maintenancesDtos = await _deviceMaintenanceService.GetDeviceMaintenancesPagedAndSortedAsync(maintenanceFilter);

                return Ok(maintenancesDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetDeviceMaintenanceByIdAsync(int Id)
        {
            if (Id == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"GetDeviceMaintenanceById with maintenance id: {Id}");
                var maintenance = await _deviceMaintenanceService.GetDeviceMaintenanceByIdAsync(Id);

                if (maintenance == null)
                {
                    _logger.LogError(common.NotFoundMaintenance);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundMaintenance);
                }

                return Ok(maintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("delete/{Id}")]
        public async Task<IActionResult> DeleteMaintenanceByIdAsync(int Id)
        {
            if (Id == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"DeleteMaintenanceById by maintenance id: {Id}");
                await _deviceMaintenanceService.DeleteDeviceMaintenanceByIdAsync(Id);

                return Ok(common.RemovedMaintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateDeviceMaintenanceAsync([FromBody] MaintenanceUpdateDto maintenanceDto)
        {
            if (maintenanceDto == null)
            {
                _logger.LogError(common.NullMaintenance);
                return BadRequest(common.NullMaintenance);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(maintenanceDto, Formatting.Indented);
                _logger.LogInformation($"UpdateDeviceMaintenance for {serializedContent}");
                var response = await _deviceMaintenanceService.UpdateDeviceMaintenanceAsync(maintenanceDto);

                if (response == ValidationResult.Null)
                {
                    _logger.LogError(common.NotFoundMaintenance);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundMaintenance);
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
        [Route("getAllMaintenances")]
        public async Task<IActionResult> GetAllMaintenancesAsync()
        {
            try
            {
                _logger.LogInformation($"GetAllDevices");
                var deviceDtos = await _deviceMaintenanceService.GetAllMaintenancesAsync();

                return Ok(deviceDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

    }
}
