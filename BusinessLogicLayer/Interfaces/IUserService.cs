using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using Common;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddUserAsync(UserDto userDto);
        Task<ValidationResult> UpdateUserAsync(UserDto userDto);
        Task<IEnumerable<UserGetDto>> GetAllUsersAsync();
        Task DeleteUserByIdAsync(int userId);
        Task<UserGetDto> GetUserByIdAsync(int userId);
        Task<PagedResponse<UserInListDto>> GetUsersFilteredPagedAsync(UserParameters userParameters);

        UserDto ConvertUserToDto(User user);
    }
}
