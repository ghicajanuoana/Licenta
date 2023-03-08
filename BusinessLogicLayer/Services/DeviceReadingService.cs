using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLogicLayer.Services
{
    public class DeviceReadingService : IDeviceReadingService
    {
        private readonly IDeviceReadingRepository _deviceReadingRepository;
        private IMemoryCache _memoryCache;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        public DeviceReadingService(IDeviceReadingRepository deviceReadingRepository, IMemoryCache memoryCache)
        {
            _deviceReadingRepository = deviceReadingRepository;
            _memoryCache = memoryCache;
        }

        public async Task<bool> AddDeviceReadingAsync(DeviceReadingDto deviceReadingDto)
        {
            if (!_memoryCache.TryGetValue(deviceReadingDto.DeviceReadingTypeName, out DeviceReadingType readingType))
            {
                await _semaphoreSlim.WaitAsync();
                try
                {
                    if (!_memoryCache.TryGetValue(deviceReadingDto.DeviceReadingTypeName, out readingType))
                    {
                        readingType = await _deviceReadingRepository.GetReadingTypeByName(deviceReadingDto.DeviceReadingTypeName);
                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(15),
                            SlidingExpiration = TimeSpan.FromMinutes(5),
                            Size = 1024,
                        };

                        _memoryCache.Set(deviceReadingDto.DeviceReadingTypeName, readingType, cacheEntryOptions);
                    }
                }
                finally 
                { 
                    _semaphoreSlim.Release(); 
                }
            }
                
            if (readingType != null)
            {
                var deviceReadingType = ConvertDtoToDeviceReading(deviceReadingDto);
                deviceReadingType.DeviceReadingTypeId = readingType.Id;
                await _deviceReadingRepository.AddDeviceReadingAsync(deviceReadingType);
                return true;
            }

            return false;
        }

        public async Task MarkAlertAsReadAsync(MarkAlertReadDeviceReadingDto markedAlertDto)
        {
            try
            {
                await _deviceReadingRepository.MarkAlertAsReadAsync(markedAlertDto.DeviceReadingIds);
            }
            catch
            {
                throw new Exception("Invalid device reading !");
            }
        }

        public async Task<int> GetUnreadAlertsCountAsync()
        {
            var unreadAlerts = await _deviceReadingRepository.GetUnreadAlertsCountAsync();

            return unreadAlerts;
        }

        public static DeviceReading ConvertDtoToDeviceReading(DeviceReadingDto deviceReadingDto)
        {
            var deviceReading = new DeviceReading()
            {
                DeviceId = deviceReadingDto.DeviceId,
                Id = deviceReadingDto.Id,
                ReceivedTs = deviceReadingDto.ReceivedTs,
                ValueRead = deviceReadingDto.ValueRead
            };

            return deviceReading;
        }
    }
}