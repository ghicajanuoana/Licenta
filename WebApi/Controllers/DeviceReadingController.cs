using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebApi.Hubs;

namespace WebApi.Controllers
{ /*
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceReadingController : ControllerBase
    {
        private readonly IDeviceReadingService _deviceReadingService;
        private readonly IHubContext<AlertHub> _hubContext;
        private readonly ILogger<DeviceReadingController> _logger;

        CommonStrings common = new CommonStrings();

        public DeviceReadingController(IDeviceReadingService deviceReadingService, IHubContext<AlertHub> hubContext
            , ILogger<DeviceReadingController> logger)
        {
            _deviceReadingService = deviceReadingService;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddDeviceReading")]
        public async Task<IActionResult> AddDeviceReadingAsync([FromBody] DeviceReadingDto deviceReading)
        {
            if (deviceReading == null)
            {
                _logger.LogError(common.NullDeviceReading);
                return StatusCode(SpecialCodes.Status702NotFound, common.NullDeviceReading);
            }
            try
            {

                var serializedContent = JsonConvert.SerializeObject(deviceReading, Formatting.Indented);
                _logger.LogInformation($"AddDeviceReading for {serializedContent}");
                if (!await _deviceReadingService.AddDeviceReadingAsync(deviceReading))
                {
                    return StatusCode(SpecialCodes.Status702NotFound, common.InvalidName);
                }

                var unreadAlertsCount = await _deviceReadingService.GetUnreadAlertsCountAsync();

                await _hubContext.Clients.All.SendAsync("ReceiveUnreadAlertCount", unreadAlertsCount);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("alertRead")]
        public async Task<IActionResult> MarkAlertReadAsync([FromBody] MarkAlertReadDeviceReadingDto markedDevicesDto)
        {
            var serializedContent = JsonConvert.SerializeObject(markedDevicesDto, Formatting.Indented);
            if (markedDevicesDto.DeviceReadingIds.Any(d => d == 0))
            {
                _logger.LogError(common.NullDeviceReading + $" for {serializedContent}");
                return StatusCode(SpecialCodes.Status702NotFound, common.NullDeviceReading);
            }

            try
            {
                _logger.LogInformation($"MarkAlertRead for {serializedContent}");
                await _deviceReadingService.MarkAlertAsReadAsync(markedDevicesDto);

                var unreadAlertsCount = await _deviceReadingService.GetUnreadAlertsCountAsync();

                await _hubContext.Clients.All.SendAsync("ReceiveUnreadAlertCount", unreadAlertsCount);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }

    */
}