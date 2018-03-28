using System;
using System.Collections.Generic;
using System.Text;
using NetCorePoc.Domain.Entities;

namespace NetCorePoc.Domain.Interfaces.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();

        User GetUserById(int id);

        int InsertUser(User user);

        bool UpdatetUser(int id, User user);

        bool DeleteUser(int id);
    }
}
