using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Interfaces;
using Common;
using DataAccessLayer.Enums;
using DataAccessLayer.FilterModels;
using DataAccessLayer.Implementation;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services
{
    public class DeviceMaintenanceService : IDeviceMaintenanceService
    {
        private readonly IDeviceMaintenanceRepository _deviceMaintenanceRepository;
        private readonly IDeviceService _deviceService;
        private const string CreatedBy = "Admin";
        CommonStrings common = new CommonStrings();

        public DeviceMaintenanceService(IDeviceMaintenanceRepository deviceMaintenanceRepository, IDeviceService deviceService)
        {
            this._deviceMaintenanceRepository = deviceMaintenanceRepository;
            this._deviceService = deviceService;
        }

        public async Task<bool> AddDeviceMaintenanceAsync(MaintenanceAddDto maintenanceDto)
        {
            //if (_deviceMaintenanceRepository.IsDeviceUsed(maintenanceDto.DeviceId))
            
                var maintenance = ConvertDtoToMaintenance(maintenanceDto);
                await _deviceMaintenanceRepository.AddDeviceMaintenanceAsync(maintenance);
                return true;
            
            //return false;
        }

        public async Task DeleteDeviceMaintenanceByIdAsync(int deviceMaintenanceId)
        {
            if (deviceMaintenanceId == 0)
            {
                throw new KeyNotFoundException(common.NotFoundMaintenance);
            }

            await _deviceMaintenanceRepository.DeleteDeviceMaintenanceByIdAsync(deviceMaintenanceId);
        }

        public async Task<MaintenanceDto> GetDeviceMaintenanceByIdAsync(int deviceMaintenanceId)
        {
            var deviceMaintenance = await _deviceMaintenanceRepository.GetDeviceMaintenanceByIdAsync(deviceMaintenanceId);

            return (deviceMaintenance == null) ? null : ConvertMaintenanceToDto(deviceMaintenance);
        }

        public async Task<ValidationResult> UpdateDeviceMaintenanceAsync(MaintenanceUpdateDto maintenanceDto)
        {

            var maintenance = await _deviceMaintenanceRepository.GetDeviceMaintenanceByIdAsync(maintenanceDto.Id);
            if (maintenance != null)
            {
                var maintenanceUpdated = ConvertUpdateDtoToMaintenance(maintenanceDto, maintenance);
                await _deviceMaintenanceRepository.UpdateDeviceMaintenanceAsync(maintenanceUpdated);
                return ValidationResult.Success;
            }
            return ValidationResult.Null;
        }

        public async Task<PagedResponse<MaintenanceDto>> GetDeviceMaintenancesPagedAndSortedAsync(MaintenanceFilter maintenanceFilter)
        {
            var deviceMaintenances = await _deviceMaintenanceRepository.GetDeviceMaintenanceOrderedAndFilteredAsync(maintenanceFilter);

            var count = deviceMaintenances.Count();

            switch (maintenanceFilter.PagingFilteringParameters.OrderBy)
            {
                case "Device":
                    deviceMaintenances = maintenanceFilter.PagingFilteringParameters.OrderDescending
                        ? deviceMaintenances.OrderByDescending(m => m.Device.Name)
                        : deviceMaintenances.OrderBy(m => m.Device.Name);
                    break;

                case "Description":
                    deviceMaintenances = maintenanceFilter.PagingFilteringParameters.OrderDescending
                        ? deviceMaintenances.OrderByDescending(m => m.Description)
                        : deviceMaintenances.OrderBy(m => m.Description);
                    break;

                case "Status":
                    deviceMaintenances = maintenanceFilter.PagingFilteringParameters.OrderDescending
                        ? deviceMaintenances.OrderByDescending(m => m.Status)
                        : deviceMaintenances.OrderBy(m => m.Status);
                    break;

                case "ScheduledDate":
                    deviceMaintenances = maintenanceFilter.PagingFilteringParameters.OrderDescending
                        ? deviceMaintenances.OrderByDescending(m => m.ScheduledDate)
                        : deviceMaintenances.OrderBy(m => m.ScheduledDate);
                    break;

                case "ActualDate":
                    deviceMaintenances = maintenanceFilter.PagingFilteringParameters.OrderDescending
                        ? deviceMaintenances.OrderByDescending(m => m.ActualDate)
                        : deviceMaintenances.OrderBy(m => m.ActualDate);
                    break;

                case "CreatedAt":
                    deviceMaintenances = maintenanceFilter.PagingFilteringParameters.OrderDescending
                        ? deviceMaintenances.OrderByDescending(m => m.CreatedAt)
                        : deviceMaintenances.OrderBy(m => m.CreatedAt);
                    break;

                case "CreatedBy":
                    deviceMaintenances = maintenanceFilter.PagingFilteringParameters.OrderDescending
                        ? deviceMaintenances.OrderByDescending(m => m.CreatedBy)
                        : deviceMaintenances.OrderBy(m => m.CreatedBy);
                    break;
            }

            var result = deviceMaintenances
                .Skip(maintenanceFilter.PagingFilteringParameters.PageNumber * maintenanceFilter.PagingFilteringParameters.PageSize)
                .Take(maintenanceFilter.PagingFilteringParameters.PageSize);

            var deviceMaintenanceDtos = new List<MaintenanceDto>();

            foreach (var deviceMaintenance in result)
            {
                var deviceMaintenanceDto = ConvertMaintenanceToDto(deviceMaintenance);

                deviceMaintenanceDtos.Add(deviceMaintenanceDto);
            }
            return new PagedResponse<MaintenanceDto>(deviceMaintenanceDtos, maintenanceFilter.PagingFilteringParameters.PageNumber, maintenanceFilter.PagingFilteringParameters.PageSize, count);
        }

        private Maintenance ConvertUpdateDtoToMaintenance(MaintenanceUpdateDto maintenanceDto, Maintenance maintenance)
        {
            maintenance.DeviceId = maintenanceDto.DeviceId;
            maintenance.Description = maintenanceDto.Description;
            maintenance.Outcome = maintenanceDto.Outcome;
            maintenance.ScheduledDate = maintenanceDto.ScheduledDate;
            maintenance.ActualDate= maintenanceDto.ActualDate;
            maintenance.Status = maintenanceDto.Status;
            return maintenance;
        }

        private Maintenance ConvertDtoToMaintenance(MaintenanceAddDto maintenanceDto)
        {
            var maintenance = new Maintenance();
            maintenance.Id = maintenanceDto.Id;
            maintenance.DeviceId = maintenanceDto.DeviceId;
            maintenance.ScheduledDate = maintenanceDto.ScheduledDate;
            maintenance.Description = maintenanceDto.Description;
            maintenance.CreatedBy = CreatedBy;
            maintenance.CreatedAt = DateTime.UtcNow;
            maintenance.Status = Status.New;
            return maintenance;
        }

        private MaintenanceDto ConvertMaintenanceToDto(Maintenance maintenance)
        {
            var maintenanceDto = new MaintenanceDto()
            {
                Id = maintenance.Id,
                MaintenanceDeviceDto = _deviceService.ConvertDeviceToMaintenanceDeviceDto(maintenance.Device),
                ScheduledDate = maintenance.ScheduledDate,
                ActualDate = maintenance.ActualDate,
                Status = maintenance.Status.ToString(),
                CreatedAt = maintenance.CreatedAt,
                CreatedBy = maintenance.CreatedBy,
                Outcome = maintenance.Outcome,
                Description = maintenance.Description
            };

            return maintenanceDto;
        }

        public async Task<IEnumerable<MaintenanceDto>> GetAllMaintenancesAsync()
        {
            var allMFromDb = await _deviceMaintenanceRepository.GetAllMaintenancesAsync();
            var maintenanceDtos = new List<MaintenanceDto>();
            foreach (var m in allMFromDb)
            {
                var maintenanceDto = ConvertMaintenanceToDto(m);
                maintenanceDtos.Add(maintenanceDto);
            }
            return maintenanceDtos;
        }
    }
}
