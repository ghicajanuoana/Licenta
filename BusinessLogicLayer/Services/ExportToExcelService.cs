using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SwiftExcel;
using Syroot.Windows.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace BusinessLogicLayer.Services
{
    public class ExportToExcelService : IExportToExcelService
    {
        public async Task ExportToExcel(IEnumerable<DeviceDto> deviceDtos)
        {
            var hourMinute = "\\Devices" + DateTime.Now.ToString(" dd-MM-yy HH-mm-ss");
            var path = DefaultDownloadPath() + hourMinute + ".xlsx";
            var newFile = @path;

            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet("Sheet1");
                var count = deviceDtos.Count();
                var rowIndex = 0;
                IRow row = sheet1.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("Device Id");
                row.CreateCell(1).SetCellValue("Device Name");
                row.CreateCell(2).SetCellValue("Serial Number");
                row.CreateCell(3).SetCellValue("Location Name");
                row.CreateCell(4).SetCellValue("DeviceType Name");
                row.CreateCell(5).SetCellValue("Description");
                rowIndex++;
                row = sheet1.CreateRow(rowIndex);
                foreach (var device in deviceDtos)
                {
                    var col = 0;
                    row.CreateCell(col++).SetCellValue(device.DeviceId);
                    row.CreateCell(col++).SetCellValue(device.Name);
                    row.CreateCell(col++).SetCellValue(device.SerialNumber);
                    row.CreateCell(col++).SetCellValue(device.Location.Name);
                    row.CreateCell(col++).SetCellValue(device.DeviceType.Name);
                    row.CreateCell(col++).SetCellValue(device.Description);
                    rowIndex++;
                    row = sheet1.CreateRow(rowIndex);
                }
                workbook.Write(fs);
            }
        }

        public async Task ExportToExcelSwiftExcel(IEnumerable<DeviceDto> deviceDtos)
        {
            var count = deviceDtos.Count();
            var hourMinute = "\\Devices" + DateTime.Now.ToString(" dd-MM-yy HH-mm-ss");
            var path = DefaultDownloadPath() + hourMinute + ".xlsx";

            using (var ew = new ExcelWriter(@path))
            {
                var row = 2;
                var col = 1;

                ew.Write("Device Id", col++, 1);
                ew.Write("Device Name", col++, 1);
                ew.Write("Serial Number", col++, 1);
                ew.Write("Location Name", col++, 1);
                ew.Write("DeviceType Name", col++, 1);

                foreach (var device in deviceDtos)
                {
                    col = 1;
                    ew.Write(Convert.ToString(device.DeviceId), col++, row);
                    ew.Write(Convert.ToString(device.Name), col++, row);
                    ew.Write(Convert.ToString(device.SerialNumber), col++, row);
                    ew.Write(Convert.ToString(device.Location.Name), col++, row);
                    ew.Write(Convert.ToString(device.DeviceType.Name), col++, row);
                    row++;
                }
            }
        }

        private static WindowsIdentity WindowsIdentityFromProcess(Process process)
        {
            var process_handle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out process_handle);
                return new WindowsIdentity(process_handle);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (process_handle != IntPtr.Zero)
                {
                    CloseHandle(process_handle);
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr process_handle, uint desired_access, out IntPtr token_handle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr h_object);

        private static string DefaultDownloadPath()
        {
            var windows_identity = WindowsIdentityFromProcess(Process.GetProcessesByName("explorer").FirstOrDefault());
            return windows_identity != null ? new KnownFolder(KnownFolderType.Downloads, windows_identity).Path : KnownFolders.PublicDownloads.Path;
        }
    }
}

