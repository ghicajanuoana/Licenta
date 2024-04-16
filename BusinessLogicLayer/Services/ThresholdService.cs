using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces; 
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Services
{
    public class ThresholdService : IThresholdService
    {
        private readonly IThresholdRepository _thresholdRepository;
        private readonly IDeviceTypeRepository _deviceTypeRepository;
        private readonly IDeviceReadingTypeRepository _deviceReadingTypeRepository;
        private readonly IDeviceTypeService _deviceTypeService;
        private readonly IDeviceReadingTypeService _deviceReadingTypeService;
        CommonStrings common = new CommonStrings();

        public ThresholdService(IThresholdRepository thresholdRepository, IDeviceTypeRepository deviceTypeRepository, IDeviceReadingTypeRepository deviceReadingTypeRepository, IDeviceTypeService deviceTypeService, IDeviceReadingTypeService deviceReadingTypeService)
        {
            _thresholdRepository = thresholdRepository;
            _deviceTypeRepository = deviceTypeRepository;
            _deviceReadingTypeRepository = deviceReadingTypeRepository;
            _deviceTypeService = deviceTypeService;
            _deviceReadingTypeService = deviceReadingTypeService;
        }

        public async Task<bool> AddThresholdAsync(ThresholdDto newThresholdDto)
        {
            var devicesIdExists = await _thresholdRepository.DevicesIdAlreadyExistsInThresholdAsync(newThresholdDto.DeviceType.DeviceTypeId, newThresholdDto.DeviceReadingType.DeviceReadingTypeId);
            var deviceTypeIdExists = await _deviceTypeRepository.DeviceTypeIdExistsAsync(newThresholdDto.DeviceType.DeviceTypeId);
            var deviceReadingTypeIdExists = await _deviceReadingTypeRepository.DeviceReadingTypeIdExistsAsync(newThresholdDto.DeviceReadingType.DeviceReadingTypeId);

            if (deviceTypeIdExists && deviceReadingTypeIdExists)
            {
                if (!devicesIdExists)
                {
                    var threshold =  ConvertDtoToThreshold(newThresholdDto);
                    await _thresholdRepository.AddThresholdAsync(threshold);
                    return true;
                }
            }
            return false;
        }

        public async Task<ValidationResult> UpdateThresholdAsync(ThresholdDto thresholdDto)
        {
            if (thresholdDto == null)
            {
                return ValidationResult.Null;
            }

            var isNewThresholdFound = await _thresholdRepository.IsFoundDuplicateThreshold(thresholdDto.DeviceType.DeviceTypeId, thresholdDto.DeviceReadingType.DeviceReadingTypeId, thresholdDto.Id);

            if (!isNewThresholdFound)
            {
                var threshold = ConvertDtoToThreshold(thresholdDto);
                await _thresholdRepository.UpdateThresholdAsync(threshold);
                return ValidationResult.Success;
            }
            return ValidationResult.InUse;
        }

        public async Task<IEnumerable<ThresholdDto>> GetAllThresholdsAsync()
        {
            var allThresholds = await _thresholdRepository.GetAllThresholdsAsync();
            var thresholdDtos = new List<ThresholdDto>();
            foreach (var threshold in allThresholds)
            {
                var thresholdDto = ConvertThresholdToDto(threshold);
                thresholdDtos.Add(thresholdDto);
            }
            return thresholdDtos;
        }

        public async Task DeleteThresholdByIdAsync(int thresholdId)
        {
            if (thresholdId == 0)
            {
                throw new KeyNotFoundException(common.NotFoundThreshold);
            }
           
            await _thresholdRepository.DeleteThresholdByIdAsync(thresholdId);
        }

        public async Task<ThresholdDto> GetThresholdByIdAsync(int thresholdId)
        {
            var threshold = await _thresholdRepository.GetThresholdByIdAsync(thresholdId);

            if (threshold == null)
            {
                return null;
            }

            var thresholdDto = ConvertThresholdToDto(threshold);

            return thresholdDto;
        }

        private ThresholdDto ConvertThresholdToDto(Threshold threshold)
        {
            var thresholdDto = new ThresholdDto()
            {
                Id = threshold.Id,
                DeviceType = _deviceTypeService.ConvertDeviceTypeToDto(threshold.DeviceType),
                MinValue = threshold.MinValue,
                MaxValue = threshold.MaxValue,
                WarningValue = threshold.WarningValue,
                CriticalValue = threshold.CriticalValue,
                DeviceReadingType = _deviceReadingTypeService.ConvertDeviceReadingTypeToDto(threshold.DeviceReadingType)
            };
            return thresholdDto;
        }

        private static Threshold ConvertDtoToThreshold(ThresholdDto thresholdDto)
        {
            var threshold = new Threshold();
            threshold.Id = thresholdDto.Id;
            threshold.MaxValue = thresholdDto.MaxValue;
            threshold.MinValue = thresholdDto.MinValue;
            threshold.WarningValue = thresholdDto.WarningValue;
            threshold.CriticalValue = thresholdDto.CriticalValue;
            threshold.DeviceReadingTypeId = thresholdDto.DeviceReadingType.DeviceReadingTypeId;
            threshold.DeviceTypeId = thresholdDto.DeviceType.DeviceTypeId;
            return threshold;
        }
    }
}
