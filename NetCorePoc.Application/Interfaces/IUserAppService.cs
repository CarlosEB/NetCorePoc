using System.Collections.Generic;
using NetCorePoc.Application.DTOs;

namespace NetCorePoc.Application.Interfaces
{
    public interface IUserAppService
    {
        IEnumerable<UserResponse> GetUsers();

        UserResponse GetUserById(int id);

        int InsertUser(UserRequest user);

        bool UpdatetUser(int id, UserRequest user);

        bool DeleteUser(int id);
    }
}
