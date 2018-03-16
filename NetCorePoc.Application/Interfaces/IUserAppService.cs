﻿using System.Collections.Generic;
using NetCorePoc.Application.DTOs;

namespace NetCorePoc.Application.Interfaces
{
    public interface IUserAppService
    {
        IEnumerable<UserOutput> GetUsers();

        UserOutput GetUserById(int id);

        int InsertUser(UserInput user);

        bool UpdatetUser(int id, UserInput user);

        bool DeleteUser(int id);
    }
}
