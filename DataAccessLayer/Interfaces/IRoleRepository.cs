using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();

        Task<bool> DeviceTypeIdExistsAsync(int deviceTypeId);

        Task<Role> GetRoleByType(string roleType);
    }
}
