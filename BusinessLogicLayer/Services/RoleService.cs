using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            var rolesDto = new List<RoleDto>();

            foreach (var role in roles)
            {
                var roleDto = ConvertRoleToDto(role);
                rolesDto.Add(roleDto);
            }

            return rolesDto;
        }

        public static RoleDto ConvertRoleToDto(Role role)
        {
            var roleDto = new RoleDto
            {
                Id = role.Id,
                RoleType = role.RoleType,
            };

            return roleDto;
        }

        public static Role ConvertDtoToRole(RoleDto roleDto)
        {
            var role = new Role
            {
                Id = roleDto.Id,
                RoleType = roleDto.RoleType,
            };

            return role;
        }
    }
}