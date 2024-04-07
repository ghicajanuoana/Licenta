using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _dataContext;

        public RoleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DeviceTypeIdExistsAsync(int roleId)
        {
            return await _dataContext.Roles.AnyAsync(r => r.Id == roleId);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _dataContext.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByType(string roleType)
        {
            return await _dataContext.Roles
                .Where(r => r.RoleType.Equals(roleType))
                .FirstOrDefaultAsync();
        }
    }
}