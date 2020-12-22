using System;
using System.Threading.Tasks;

namespace M365.RoadMapInfo.Authentication
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }
}