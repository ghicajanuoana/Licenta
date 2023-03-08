using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Hubs
{
    public class AlertHub : Hub
    {
        private readonly IDeviceReadingService _deviceReadingService;

        public AlertHub(IDeviceReadingService deviceReadingService)
        {
            _deviceReadingService = deviceReadingService;
        }

        public async Task GetUnreadAlertCount()
        {
            var unreadAlertCount = await _deviceReadingService.GetUnreadAlertsCountAsync();

            await Clients.All.SendAsync("ReceiveUnreadAlertCount", unreadAlertCount);
        }
    }
}
