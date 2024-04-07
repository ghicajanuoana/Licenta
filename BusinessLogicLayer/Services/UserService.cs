using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using BusinessLogicLayer.Enums;
using DataAccessLayer.Implementation;
using static BusinessLogicLayer.Services.RoleService;
using Common;

namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;

        public UserService(IUserRepository userRepository, IRoleService roleService)
        {
            _userRepository = userRepository;
            _roleService = roleService;
        }

        public async Task<bool> AddUserAsync(UserDto userDto)
        {
            if (_userRepository.IsUserNameUnique(userDto.UserId, userDto.Username))
            {
                var user = ConvertDtoToUser(userDto);
                await _userRepository.AddUserAsync(user);

                return true;
            }

            return false;
        }

        public async Task<ValidationResult> UpdateUserAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                return ValidationResult.Null;
            }

            if (_userRepository.IsUserNameUnique(userDto.UserId, userDto.Username))
            {
                var user = ConvertDtoToUser(userDto);
                await _userRepository.UpdateUserAsync(user);
                return ValidationResult.Success;
            }

            return ValidationResult.InUse;
        }

        private User ConvertDtoToUser(UserDto userDto)
        {
            var user = new User
            {
                Id = userDto.UserId,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Username = userDto.Username,
                Password = userDto.Password,
                RoleId = userDto.RoleId,
                IsActive = userDto.IsActive,
            };

            return user;
        }

        private UserDto ConvertUserToDto(User user)
        {
            return new UserDto()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
                //RoleId = user.RoleId,
                //IsActive = user.IsActive
            };
        }

        private UserGetDto ConvertUserToGetDto(User user)
        {
            return new UserGetDto()
            {
                UserId = user.Id,
                Username = user.Username,
                Role = ConvertRoleToDto(user.Role),
                //IsActive = user.IsActive
            };
        }

        public async Task<IEnumerable<UserGetDto>> GetAllUsersAsync()
        {
            var allUsers = await _userRepository.GetAllUsersAsync();
            var usersDto = new List<UserGetDto>();

            foreach (var user in allUsers)
            {
                var userDto = ConvertUserToGetDto(user);
                usersDto.Add(userDto);
            }
            return usersDto;
        }

        public async Task<UserGetDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return (user == null) ? null : ConvertUserToGetDto(user);
        }

        public async Task DeleteUserByIdAsync(int userId)
        {
            if (userId == 0)
            {
                throw new KeyNotFoundException("This user doesn't exist");
            }

            await _userRepository.DeleteUserByIdAsync(userId);
        }

        private UserInListDto ConvertUserToInListDto(User user)
        {
            var usersDto = new UserInListDto
            {
                UserId = user.Id,
                Username = user.Username,
                //IsActive = user.IsActive,
                Role = ConvertRoleToDto(user.Role)
            };
            return usersDto;
        }

        public async Task<PagedResponse<UserInListDto>> GetUsersFilteredPagedAsync(UserParameters userParameters)
        {
            if (userParameters.PagingFilteringParameters == null)
            {
                userParameters.PagingFilteringParameters = new PagingFilteringParameters();
            }

            var users = await _userRepository.GetUsersFilteredPagedAsync(userParameters);

            var userDto = new List<UserInListDto>();
            foreach (var user in users.Data)
            {
                var usersDto = ConvertUserToInListDto(user);
                userDto.Add(usersDto);
            }
                return new PagedResponse<UserInListDto>(userDto, userParameters.PagingFilteringParameters.PageNumber,
                    users.PageSize, users.TotalCount);
            }

        UserDto IUserService.ConvertUserToDto(User user)
        {
            return new UserDto()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
                //RoleId = user.RoleId,
                //IsActive = user.IsActive
            };
        }
    }
    }


