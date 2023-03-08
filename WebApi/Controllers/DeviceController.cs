using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Interfaces;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using SwiftExcel;
using System.Reflection;
using System;
using DataAccessLayer.Models;
using DataAccessLayer.Migrations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<DeviceController> _logger;
        CommonStrings common = new CommonStrings();

        public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger)
        {
            this._deviceService = deviceService;
            _logger = logger;
        }

        [HttpGet]
        [Route("getAllDevices")]
        public async Task<IActionResult> GetAllDevicesAsync()
        {
            try
            {
                _logger.LogInformation($"GetAllDevices");
                var deviceDtos = await _deviceService.GetAllDevicesAsync();

                return Ok(deviceDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getDevicesByLocationId/{locationId}")]
        public async Task<IActionResult> GetAllDevicesByLocationIdAsync(int locationId)
        {
            if (locationId == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest("Invalid location id !");
            }

            try
            {
                _logger.LogInformation($"GetAllDevicesByLocationId with location id: {locationId}");
                var deviceDtos = await _deviceService.GetAllDevicesByLocationIdAsync(locationId);
                return Ok(deviceDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("exportToCSV")]
        public async Task<IActionResult> ExportToCSV([FromQuery] DeviceParameters deviceParameters)
        {
            try
            {

                _logger.LogInformation($"ExportToCSV"); ;
                var stream = await _deviceService.ExportToCSV(deviceParameters);

                return File(stream, "application/octet-stream", "Devices.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{deviceId}")]
        public async Task<IActionResult> GetDeviceByIdAsync(int deviceId)
        {
            if (deviceId == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"GetDeviceById with device id: {deviceId}");
                var device = await _deviceService.GetDeviceByIdAsync(deviceId);

                if (device == null)
                {
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundDevice);
                }

                return Ok(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("AddDevice")]
        public async Task<IActionResult> AddDevice([FromForm] DeviceDtoAdd device)
        {
            if (device == null)
            {
                _logger.LogError(common.NullDevice);
                return BadRequest(common.NullDevice);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(device, Formatting.Indented);
                _logger.LogInformation($"AddDevice for {serializedContent}");
                var isDeviceAdded = await _deviceService.AddDeviceAsync(device);

                if (isDeviceAdded)
                {
                    return Ok(device);
                }

                return this.StatusCode(SpecialCodes.Status700HasUniqueName, common.EnterUniqueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{deviceId}")]
        public async Task<IActionResult> DeleteDeviceByIdAsync(int deviceId)
        {
            if (deviceId == 0)
            {
                _logger.LogError(common.InvalidId + $" for {deviceId}");
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"DeleteDeviceById for {deviceId}");
                await _deviceService.DeleteDeviceByIdAsync(deviceId);

                return Ok(common.RemovedDevice);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("updateDevice")]
        public async Task<IActionResult> UpdateDeviceAsync([FromForm] DeviceDtoAdd deviceDto)
        {
            if (deviceDto == null)
            {
                _logger.LogError(common.NullDevice);
                return BadRequest(common.NullDevice);
            }

            if (string.IsNullOrEmpty(deviceDto.Name))
            {
                _logger.LogError(common.InvalidName);
                return BadRequest(common.InvalidName);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(deviceDto, Formatting.Indented);
                _logger.LogInformation($"UpdateDevice for {serializedContent}");
                var response = await _deviceService.UpdateDeviceAsync(deviceDto);

                if (response == ValidationResult.Null)
                {
                    _logger.LogError(common.NotFoundDevice);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundDevice);
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

        [HttpPut]
        [Route("receivePing")]
        public async Task<IActionResult> ReceivePingAsync([FromBody] DevicePingDto devicePingDto)
        {
            if (devicePingDto == null)
            {
                _logger.LogError(common.NotFoundDevice);
                return BadRequest(common.NotFoundDevice);
            }
            try
            {
                var serializedContent = JsonConvert.SerializeObject(devicePingDto, Formatting.Indented);
                _logger.LogInformation($"ReceivePing for {serializedContent}");
                await _deviceService.UpdateDevicePingTimestampAsync(devicePingDto);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getDevicesPagedAndFiltered")]
        public async Task<IActionResult> GetAllLocationsPagedAsync([FromQuery] DeviceParameters deviceParameters)
        {
            if (deviceParameters == null)
            {
                _logger.LogError(common.NullDeviceParameters);
                return BadRequest(common.NullDeviceParameters);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(deviceParameters, Formatting.Indented);
                _logger.LogInformation($"GetAllLocations for {serializedContent}");
                var devices = await _deviceService.GetDevicesFilteredPagedAsync(deviceParameters);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("exportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                _logger.LogInformation("ExportToExcel");
                var sentToExcel = await _deviceService.ExportToExcel();
                if (sentToExcel)
                {
                    _logger.LogInformation("Export to Excel succeded");
                    return Ok();
                }
                else
                {
                    _logger.LogError("Excel could not be downloaded");
                    return BadRequest("Excel could not be downloaded");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}