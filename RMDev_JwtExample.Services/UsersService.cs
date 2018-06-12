﻿using RMDev_JwtExample.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMDev_JwtExample.Services
{
    public class UsersService : IUsersService
    {
        private static readonly IList<User> _users = new List<User>
        {
            new User
            {
             UserId = 1,
             UserName = "Ricardo",
             DisplayName = "Ricardo Moura",
             IsActive = true,
             LastLoggedIn = null,
             Password = new SecurityService().GetSha256Hash("1234"),
             Roles = new []{ "user", "Admin" },
             SerialNumber = Guid.NewGuid().ToString("N")
            }
        };

        private readonly ISecurityService _securityService;
        public UsersService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public User FindUser(int userId)
        {
            return _users.FirstOrDefault(x => x.UserId == userId);
        }

        public User FindUser(string username, string password)
        {
            var passwordHash = _securityService.GetSha256Hash(password);
            return _users.FirstOrDefault(x => x.UserName == username && x.Password == passwordHash);
        }

        public string GetSerialNumber(int userId)
        {
            var user = FindUser(userId);
            return user.SerialNumber;
        }

        public IEnumerable<string> GetUserRoles(int userId)
        {
            var user = FindUser(userId);
            return user.Roles;
        }

        public void UpdateUserLastActivityDate(int userId)
        {
            var user = FindUser(userId);
            user.LastLoggedIn = DateTime.UtcNow;
        }
    }
}