using Common;
using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);

        Task UpdateUserAsync(User user);

        bool IsUserNameUnique(int id, string name);

        Task<IEnumerable<User>> GetAllUsersAsync();

        Task DeleteUserByIdAsync(int userId);

        Task<User> GetUserByIdAsync(int userId);

        Task<PagedResponse<User>> GetUsersFilteredPagedAsync( UserParameters user);
    }
}
