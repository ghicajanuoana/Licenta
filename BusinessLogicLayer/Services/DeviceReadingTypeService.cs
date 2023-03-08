using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services
{
    public class DeviceReadingTypeService : IDeviceReadingTypeService
    {
        private readonly IDeviceReadingTypeRepository _deviceReadingTypeRepository;
        private readonly IThresholdRepository _thresholdRepository;

        public DeviceReadingTypeService(IDeviceReadingTypeRepository deviceReadingTypeRepository, IThresholdRepository thresholdRepository)
        {
            _deviceReadingTypeRepository = deviceReadingTypeRepository;
            _thresholdRepository = thresholdRepository;
        }

        public async Task<IEnumerable<DeviceReadingTypeDto>> GetAllDeviceReadingTypesAsync()
        {
            var deviceReadingTypes = await _deviceReadingTypeRepository.GetAllDeviceReadingTypesAsync();
            var deviceReadingTypeDtos = new List<DeviceReadingTypeDto>();

            foreach (var deviceReadingType in deviceReadingTypes)
            {
                var deviceReadingTypeDto = ConvertDeviceReadingTypeToDto(deviceReadingType);

                deviceReadingTypeDtos.Add(deviceReadingTypeDto);
            }
            return deviceReadingTypeDtos;
        }

        public async Task<bool> AddDeviceReadingTypeAsync(DeviceReadingTypeDto deviceReadingTypeDto)
        {
            if (_deviceReadingTypeRepository.IsDeviceReadingTypeNameUnique(deviceReadingTypeDto.DeviceReadingTypeId, deviceReadingTypeDto.Name))
            {
                var deviceReadingType = ConvertDtoToDeviceReadingType(deviceReadingTypeDto);

                await _deviceReadingTypeRepository.AddDeviceReadingTypeAsync(deviceReadingType);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteDeviceReadingTypeByIdAsync(int deviceReadingTypeId)
        {
            var isDeviceReadingTypeUsedAsync = await _thresholdRepository.IsDeviceReadingTypeUsedAsync(deviceReadingTypeId);

            if (!isDeviceReadingTypeUsedAsync)
            {
                await _deviceReadingTypeRepository.DeleteDeviceReadingTypeByIdAsync(deviceReadingTypeId);
            }

            return isDeviceReadingTypeUsedAsync;
        }

        public async Task<bool> IsDeviceReadingTypeUsedAsync(int deviceReadingTypeId)
        {
            return await _thresholdRepository.IsDeviceReadingTypeUsedAsync(deviceReadingTypeId);
        }

        public async Task<ValidationResult> UpdateDeviceReadingTypeAsync(DeviceReadingTypeDto deviceReadingTypeDto)
        {
            var deviceReadingType = await _deviceReadingTypeRepository.GetDeviceReadingTypeByIdAsync(deviceReadingTypeDto.DeviceReadingTypeId);

            if (deviceReadingType == null)
            {
                return ValidationResult.Null;
            }
            else if (_deviceReadingTypeRepository.IsDeviceReadingTypeNameUnique(deviceReadingTypeDto.DeviceReadingTypeId, deviceReadingTypeDto.Name))
            {
                deviceReadingType = ConvertDtoToDeviceReadingType(deviceReadingTypeDto, deviceReadingType);
                await _deviceReadingTypeRepository.UpdateDeviceReadingTypeAsync(deviceReadingType);
                return ValidationResult.Success;
            }
            return ValidationResult.NotUnique;
        }

        public DeviceReadingType ConvertDtoToDeviceReadingType(DeviceReadingTypeDto deviceReadingTypeDto, DeviceReadingType dbDeviceReadingType = null)
        {
            DeviceReadingType deviceReadingType = dbDeviceReadingType ?? new DeviceReadingType();
            deviceReadingType.Name = deviceReadingTypeDto.Name;
            deviceReadingType.Unit = deviceReadingTypeDto.Unit;
            return deviceReadingType;
        }

        public DeviceReadingTypeDto ConvertDeviceReadingTypeToDto(DeviceReadingType deviceReadingType)
        {
            DeviceReadingTypeDto deviceReadingTypeDto = new DeviceReadingTypeDto();
            deviceReadingTypeDto.DeviceReadingTypeId = deviceReadingType.Id;
            deviceReadingTypeDto.Name = deviceReadingType.Name;
            deviceReadingTypeDto.Unit = deviceReadingType.Unit;
            return deviceReadingTypeDto;
        }
    }
}