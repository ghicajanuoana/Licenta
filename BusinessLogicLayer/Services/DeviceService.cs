using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Interfaces;
using Common;
using CsvHelper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using BusinessLogicLayer.Enums;
using static BusinessLogicLayer.Services.DeviceTypeService;
using static BusinessLogicLayer.Services.LocationService;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Syroot.Windows.IO;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using SwiftExcel;
using System.Globalization;

namespace BusinessLogicLayer.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILocationService _locationService;
        private readonly IDeviceTypeService _deviceTypeService;
        private readonly IExportToExcelService _exportToExcelService;
        CommonStrings common = new CommonStrings();

        public DeviceService(IDeviceRepository deviceRepository, ILocationService locationService, IDeviceTypeService deviceTypeService,
            IExportToExcelService exportToExcelService)
        {
            _deviceRepository = deviceRepository;
            _locationService = locationService;
            _deviceTypeService = deviceTypeService;
            _exportToExcelService = exportToExcelService;
        }

        public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
        {
            var allDevicesFromDb = await _deviceRepository.GetAllDevicesAsync();
            var deviceDtos = new List<DeviceDto>();
            foreach (var device in allDevicesFromDb)
            {
                var deviceDto = ConvertDeviceToDto(device);
                deviceDtos.Add(deviceDto);
            }
            return deviceDtos;
        }

        public async Task<IEnumerable<DeviceDto>> GetAllDevicesByLocationIdAsync(int locationId)
        {
            var allDevicesFromDb = await _deviceRepository.GetAllDevicesByLocationIdAsync(locationId);
            var deviceDtos = new List<DeviceDto>();
            foreach (var device in allDevicesFromDb)
            {
                var deviceDto = ConvertDeviceToDto(device);
                deviceDtos.Add(deviceDto);
            }
            return deviceDtos;
        }

        public async Task<DeviceDtoAdd> GetDeviceByIdAsync(int deviceId)
        {
            var device = await _deviceRepository.GetDeviceByIdAsync(deviceId);

            if (device == null)
            {
                return null;
            }

            var deviceDto = ConvertDeviceToDtoAdd(device);

            return deviceDto;
        }

        public async Task<bool> AddDeviceAsync(DeviceDtoAdd newDevice)
        {
            if (await IsDeviceUniqueByLocationAsync(newDevice.Name, newDevice.DeviceId, newDevice.Location.LocationId))
            {
                var device = new Device();
                device = ConvertDtoAddToDevice(newDevice, device);
                //device = ConvertDtoAddToDevice(newDevice);
                await _deviceRepository.AddDeviceAsync(device);
                return true;
            }
            return false;
        }

        public async Task DeleteDeviceByIdAsync(int deviceId)
        {
            if (deviceId == 0)
            {
                throw new KeyNotFoundException(common.NotFoundDevice);
            }

            await _deviceRepository.DeleteDeviceByIdAsync(deviceId);
        }

        public async Task<ValidationResult> UpdateDeviceAsync(DeviceDtoAdd deviceDto)
        {
            var device = await _deviceRepository.GetDeviceByIdAsync(deviceDto.DeviceId);

            if (device == null)
            {
                return ValidationResult.Null;
            }

            else if (await IsDeviceUniqueByLocationAsync(deviceDto.Name, deviceDto.DeviceId, deviceDto.Location.LocationId))
            {
                device = ConvertDtoAddToDevice(deviceDto, device);
                await _deviceRepository.UpdateDeviceAsync(device);
                return ValidationResult.Success;
            }
            return ValidationResult.NotUnique;
        }

        private static byte[] ConvertFileToBytes(IFormFile formFile)
        {
            if (formFile != null && formFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                formFile.CopyTo(memoryStream);
                var imageBytes = memoryStream.ToArray();
                return imageBytes;
            }
            else
            {
                throw new ArgumentNullException(nameof(formFile));
            }
        }

        public static string ConvertBytesToString64(byte[] bytes)
        {
            return (bytes != null) ? Convert.ToBase64String(bytes) : string.Empty;
        }

        public MaintenanceDeviceDto ConvertDeviceToMaintenanceDeviceDto(Device device)
        {
            var maintenanceDevice = new MaintenanceDeviceDto()
            {
                DeviceId = device.DeviceId,
                DeviceName = device.Name
            };

            return maintenanceDevice;
        }

        public async Task UpdateDevicePingTimestampAsync(DevicePingDto devicePingDto)
        {
            try
            {
                var device = await _deviceRepository.GetDeviceByIdAsync(devicePingDto.DeviceId);
                device.LastPingedTs = devicePingDto.LastPingedTs;

                await _deviceRepository.UpdateDeviceAsync(device);
            }
            catch
            {
                throw new Exception(common.InvalidDevice);
            }
        }

        public async Task<PagedResponse<DeviceInListDto>> GetDevicesFilteredPagedAsync(DeviceParameters deviceParameters)
        {
            var device = new Device()
            {
                Name = deviceParameters.Name,
                SerialNumber = deviceParameters.SerialNumber,
                Description = deviceParameters.Description,
            };

            if (deviceParameters.Location != null)
            {
                var location = new Location();
                location.Name = deviceParameters.Location;
                device.Location = location;
            }

            if (deviceParameters.DeviceType != null)
            {
                var deviceType = new DeviceType();
                deviceType.Name = deviceParameters.DeviceType;
                device.DeviceType = deviceType;
            }

            if (deviceParameters.PagingFilteringParameters == null)
            {
                deviceParameters.PagingFilteringParameters = new PagingFilteringParameters();
            }

            var devices = await _deviceRepository.GetDevicesFilteredPagedAsync(deviceParameters.PagingFilteringParameters, device);

            var deviceDtos = new List<DeviceInListDto>();
            foreach (var d in devices.Data)
            {
                var deviceDto = new DeviceInListDto
                {
                    Name = d.Name,
                    Description = d.Description,
                    DeviceId = d.DeviceId,
                    DeviceType = d.DeviceType.Name,
                    Location = d.Location.Name,
                    SerialNumber = d.SerialNumber,
                    //ImageBytes = ConvertBytesToString64(d.ImageBytes)
                };
                deviceDtos.Add(deviceDto);
            }

            return new PagedResponse<DeviceInListDto>(deviceDtos, devices.PageNumber,
                devices.PageSize, devices.TotalCount);
        }

        public async Task<bool> ExportToExcel()
        {
            try
            {
                var deviceDtos = await GetAllDevicesAsync();
                await _exportToExcelService.ExportToExcel(deviceDtos);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<MemoryStream> ExportToCSV(DeviceParameters deviceParameters)
        {
            var devideDtos = await GetDevicesFilteredPagedAsync(deviceParameters);

            var stream = new MemoryStream();
            using (var streamWriter = new StreamWriter(stream, leaveOpen: true))
            {
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteField("Name");
                    csvWriter.WriteField("SerialNumber");
                    csvWriter.WriteField("Description");
                    csvWriter.WriteField("Type");
                    csvWriter.WriteField("Location");
                    await csvWriter.NextRecordAsync();

                    foreach (var value in devideDtos.Data)
                    {
                        csvWriter.WriteField(value.Name);
                        csvWriter.WriteField(value.SerialNumber);
                        csvWriter.WriteField(value.Description);
                        csvWriter.WriteField(value.DeviceType);
                        csvWriter.WriteField(value.Location);

                        await csvWriter.NextRecordAsync();
                    }
                    await streamWriter.FlushAsync();
                }
            }

            stream.Position = 0;
            return stream;
        }

        private Device ConvertDtoAddToDevice(DeviceDtoAdd deviceDto, Device dbDevice = null)
        {
            var device = dbDevice ?? new Device();
            device.Name = deviceDto.Name;
            device.SerialNumber = deviceDto.SerialNumber;
            device.Description = deviceDto.Description;
            device.LocationId = deviceDto.Location.LocationId;
            device.DeviceTypeId = deviceDto.DeviceType.DeviceTypeId;
            device.Alias = deviceDto.Alias;
            device.Emails = deviceDto.Emails;
            device.SoftwareVersion = deviceDto.SoftwareVersion;
            device.FirmwareVersion = deviceDto.FirmwareVersion;
            
            if (deviceDto.ImageFile != null)
            {
                device.ImageBytes = ConvertFileToBytes(deviceDto.ImageFile);
            }
            if (deviceDto.ImageFile == null && deviceDto.ImageBytes == null)
            {
                device.ImageBytes = null;
            }
            

            return device;
        }

        private DeviceDto ConvertDeviceToDto(Device device)
        {
            var deviceDto = new DeviceDto()
            {
                DeviceId = device.DeviceId,
                Name = device.Name,
                SerialNumber = device.SerialNumber,
                Description = device.Description,
                Location = _locationService.ConvertLocationToDto(device.Location),
                DeviceType = _deviceTypeService.ConvertDeviceTypeToDto(device.DeviceType),
                //ImageBytes = ConvertBytesToString64(device.ImageBytes)
            };
            return deviceDto;
        }

        private DeviceDtoAdd ConvertDeviceToDtoAdd(Device device)
        {
            var deviceDto = new DeviceDtoAdd()
            {
                DeviceId = device.DeviceId,
                Name = device.Name,
                SerialNumber = device.SerialNumber,
                Description = device.Description,
                Location = _locationService.ConvertLocationToDto(device.Location),
                DeviceType = _deviceTypeService.ConvertDeviceTypeToDto(device.DeviceType),
                Emails = device.Emails,
                SoftwareVersion = device.SoftwareVersion,
                FirmwareVersion = device.FirmwareVersion,
                Alias = device.Alias,
                ImageBytes = ConvertBytesToString64(device.ImageBytes)
            };
            return deviceDto;
        }

        private async Task<bool> IsDeviceUniqueByLocationAsync(string deviceName, int deviceId, int locationId)
        {
            var devices = await _deviceRepository.GetDevicesByNameAsync(deviceName);
            return devices == null || !devices.Any(d => d.LocationId == locationId && d.DeviceId != deviceId);
        }
    }
}