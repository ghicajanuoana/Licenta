using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly InternshipContext _internshipContext;

        public RoleRepository(InternshipContext internshipContext)
        {
            _internshipContext = internshipContext;
        }

        public async Task<bool> DeviceTypeIdExistsAsync(int roleId)
        {
            return await _internshipContext.Roles.AnyAsync(r => r.Id == roleId);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _internshipContext.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByType(string roleType)
        {
            return await _internshipContext.Roles
                .Where(r => r.RoleType.Equals(roleType))
                .FirstOrDefaultAsync();
        }
    }
}