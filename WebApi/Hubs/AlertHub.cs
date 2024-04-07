using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Hubs
{
    public class AlertHub : Hub
    {
        /*
        private readonly IDeviceReadingService _deviceReadingService;

        public AlertHub(IDeviceReadingService deviceReadingService)
        {
            _deviceReadingService = deviceReadingService;
        }
        */

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
