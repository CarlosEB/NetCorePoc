using System;
using System.Collections.Generic;
using NetCorePoc.Application.DTOs;
using NetCorePoc.Application.Interfaces;
using NetCorePoc.Domain.Entities;
using NetCorePoc.Domain.Interfaces.Service;

namespace NetCorePoc.Application.Apps
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserService _userService;        

        public UserAppService(IUserService userService)
        {
            _userService = userService;            
        }

        public IEnumerable<UserResponse> GetUsers()
        {
            return AutoMapper.Mapper.Map<IEnumerable<UserResponse>>(_userService.GetUsers());
        }

        public UserResponse GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            return user == null ? null : AutoMapper.Mapper.Map<UserResponse>(user);
        }

        public int InsertUser(UserRequest user)
        {
            var newUser = AutoMapper.Mapper.Map<User>(user);
            _userService.InsertUser(newUser);
            
            return newUser.Id;
        }

        public bool UpdatetUser(int id, UserRequest user)
        {
            var updatetUser = new User
            {
                Address = user.Address,
                Name = user.Name
            };

            return _userService.UpdatetUser(id, updatetUser);
        }

        public bool DeleteUser(int id)
        {
            
            return _userService.DeleteUser(id);
        }
    }
}
