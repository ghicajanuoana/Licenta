
using BusinessLogicLayer.DTOs; 

namespace BusinessLogicLayer.Interfaces
{
    public interface IExportToExcelService
    {
        Task ExportToExcel(IEnumerable<DeviceDto> deviceDtos);
        Task ExportToExcelSwiftExcel(IEnumerable<DeviceDto> deviceDtos);
    }
}
