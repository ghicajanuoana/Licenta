using Common;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddUserAsync(User user)
        {
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
        }

        public bool IsUserNameUnique(int id, string name)
        {
            return !_dataContext.Users.Any(l => l.Id != id && l.Username == name);
        }

        public async Task UpdateUserAsync(User user)
        {
            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dataContext.Users
                .Include(d => d.Role)
                //.Include(d => d.RefreshTokenExpiryTime)
                //.Include(d => d.Token)
                .ToListAsync();
        }

        public async Task DeleteUserByIdAsync(int userId)
        {
            var existingUser = await GetUserByIdAsync(userId);

            _dataContext.Users.Remove(existingUser);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dataContext.Users.Include(d => d.Role)
                .FirstOrDefaultAsync(d => d.Id == userId);
        }

        public async Task<PagedResponse<User>> GetUsersFilteredPagedAsync(UserParameters userParameters)
        { 
            var filteredUsers = _dataContext.Users
              .Include(d => d.Role)
              .Where(d => userParameters.Username == null || d.Username.Contains(userParameters.Username))
              .Where(d => userParameters.RoleId == null || d.Role.Id==(userParameters.RoleId))
              .Where(d => (userParameters.IsActive == null) || d.IsActive.Equals(userParameters.IsActive));

            var count = filteredUsers.Count();

            IQueryable<User> orderedUsers = null;

            switch (userParameters.PagingFilteringParameters.OrderBy)
            {
                case "Name":
                    orderedUsers = userParameters.PagingFilteringParameters.OrderDescending
                        ? filteredUsers.OrderByDescending(d => d.Username)
                        : filteredUsers.OrderBy(d => d.Username);
                    break;
                case "RoleId":
                    orderedUsers = userParameters.PagingFilteringParameters.OrderDescending
                        ? filteredUsers.OrderByDescending(d => d.Role.Id)
                        : filteredUsers.OrderBy(d => d.Role.Id);
                    break;
                case "IsActive":
                    orderedUsers = userParameters.PagingFilteringParameters.OrderDescending
                        ? filteredUsers.OrderByDescending(d => d.IsActive)
                        : filteredUsers.OrderBy(d => d.IsActive);
                    break;
                case "Username":
                    orderedUsers = userParameters.PagingFilteringParameters.OrderDescending
                        ? filteredUsers.OrderByDescending(d => d.Username)
                        : filteredUsers.OrderBy(d => d.Username);
                    break;
            }

            IQueryable<User> pagedUsers = orderedUsers.Skip((userParameters.PagingFilteringParameters.PageNumber) * userParameters.PagingFilteringParameters.PageSize)
             .Take(userParameters.PagingFilteringParameters.PageSize);

            List<User> result = await pagedUsers.ToListAsync();

            return new PagedResponse<User>(result, userParameters.PagingFilteringParameters.PageNumber,
                userParameters.PagingFilteringParameters.PageSize, count);
        }
    }
}