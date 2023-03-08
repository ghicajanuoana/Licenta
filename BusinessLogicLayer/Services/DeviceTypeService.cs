using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Services
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly IDeviceTypeRepository _deviceTypeRepository;
        private readonly IDeviceRepository _deviceRepository;
        CommonStrings common = new CommonStrings();

        public DeviceTypeService(IDeviceTypeRepository deviceTypeRepository, IDeviceRepository deviceRepository)
        {
            this._deviceTypeRepository = deviceTypeRepository;
            this._deviceRepository = deviceRepository;
        }

        public DeviceTypeDto ConvertDeviceTypeToDto(DeviceType deviceType)
        {
            var deviceDto = new DeviceTypeDto()
            {
                DeviceTypeId = deviceType.Id,
                Name = deviceType.Name
            };
            return deviceDto;
        }

        public static DeviceType ConvertDtoToDeviceType(DeviceTypeDto deviceTypeDto, DeviceType dbDeviceType = null)
        {
            var deviceType = dbDeviceType ?? new DeviceType();
            deviceType.Name = deviceTypeDto.Name;

            return deviceType;
        }

        public async Task<IEnumerable<DeviceTypeDto>> GetAllDeviceTypesAsync()
        {
            var deviceTypesFromDb = await _deviceTypeRepository.GetAllDeviceTypesAsync();
            var deviceTypeDtos = new List<DeviceTypeDto>();

            foreach (var deviceType in deviceTypesFromDb)
            {
                var devideTypeDto = new DeviceTypeDto
                {
                    DeviceTypeId = deviceType.Id,
                    Name = deviceType.Name
                };

                deviceTypeDtos.Add(devideTypeDto);
            }

            return deviceTypeDtos;
        }

        public async Task<DeviceTypeDto> GetDeviceTypeByNameAsync(string deviceTypeName)
        {
            var deviceType = await _deviceTypeRepository.GetDeviceTypeByNameAsync(deviceTypeName);

            if (deviceType == null)
            {
                return null;
            }
            var deviceTypeDto = new DeviceTypeDto
            {
                DeviceTypeId = deviceType.Id,
                Name = deviceType.Name
            };

            return deviceTypeDto;
        }

        public async Task<bool> AddDeviceTypeAsync(DeviceTypeDto deviceTypeDto)
        {
            var isUnique = await _deviceTypeRepository.ValidateUniqueNameAsync(deviceTypeDto.Name);

            if (isUnique)
            {
                var deviceType = new DeviceType
                {
                    Name = deviceTypeDto.Name
                };

                await _deviceTypeRepository.AddDeviceTypeAsync(deviceType);
            }

            return isUnique;
        }

        public async Task<ValidationResult> UpdateDeviceTypeAsync(DeviceTypeDto deviceTypeDto)
        {
            var deviceType = await _deviceTypeRepository.GetDeviceTypeByIdAsync(deviceTypeDto.DeviceTypeId);

            if (deviceType == null)
            {
                return ValidationResult.Null;
            }

            if (await IsDeviceTypeNameUniqueAsync(deviceTypeDto.Name, deviceTypeDto.DeviceTypeId))
            {
                deviceType = ConvertDtoToDeviceType(deviceTypeDto, deviceType);
                await _deviceTypeRepository.UpdateDeviceTypeAsync(deviceType);
                return ValidationResult.Success;
            }

            return ValidationResult.NotUnique;
        }

        public async Task DeleteDeviceTypeByIdAsync(int deviceTypeId)
        {
            await _deviceTypeRepository.DeleteDeviceTypeByIdAsync(deviceTypeId);
        }

        public async Task CheckDeviceTypeIsUsedAsync(int deviceTypeId)
        {
            if (!_deviceRepository.IsDeviceTypeUsed(deviceTypeId))
            {
                throw new Exception(common.UsedDeviceType);
            }
        }

        private async Task<bool> IsDeviceTypeNameUniqueAsync(string name, int id)
        {
            var deviceType = await GetDeviceTypeByNameAsync(name);
            return deviceType == null || deviceType.DeviceTypeId == id;
        }
    }
}