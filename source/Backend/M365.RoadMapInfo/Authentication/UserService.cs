using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace M365.RoadMapInfo.Authentication
{
    public class UserService : IUserService
    {
        private readonly IOptions<List<ConfigUser>> _users;

        public UserService(IOptions<List<ConfigUser>> users)
        {
            _users = users;
        }

        public Task<User> Authenticate(string username, string password)
        {
            var authenticatedUser = _users.Value
                .FirstOrDefault(user => string.Equals(user.Username, username)
                                        && BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password));
            if (authenticatedUser == null) throw new Exception("invalid username or password");
            return Task.FromResult(new User
            {
                Username = authenticatedUser.Username,
                CanImport = authenticatedUser.CanImport
            });
        }
    }
}