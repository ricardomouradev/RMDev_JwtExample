using System.Collections.Generic;
using RMDev_JwtExample.DomainClasses;

namespace RMDev_JwtExample.Services
{
    public interface IUsersService
    {
        string GetSerialNumber(int userId);
        IEnumerable<string> GetUserRoles(int userId);
        User FindUser(string username, string password);
        User FindUser(int userId);
        void UpdateUserLastActivityDate(int userId);
    }
}