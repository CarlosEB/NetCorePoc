using System;
using System.Collections.Generic;
using System.Linq;
using NetCorePoc.Application.DTOs;
using NetCorePoc.Application.Interfaces;
using NetCorePoc.Domain.Entities;
using NetCorePoc.Domain.Interfaces.Repositories;
using NetCorePoc.Domain.Interfaces.UnitOfWork;

namespace NetCorePoc.Application.Apps
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRpo;
        private readonly IUnitOfWork _uow;

        public UserAppService(IUserRepository userRepo, IUnitOfWork uow)
        {
            _userRpo = userRepo;
            _uow = uow;
        }

        public IEnumerable<UserOutput> GetUsers()
        {
            return AutoMapper.Mapper.Map<IEnumerable<UserOutput>>(_userRpo.GetAll());
        }

        public UserOutput GetUserById(int id)
        {
            var user = _userRpo.GetWhere(w => w.Id == id).FirstOrDefault();
            return user == null ? null : AutoMapper.Mapper.Map<UserOutput>(user);
        }

        public int InsertUser(UserInput user)
        {
            var newUser = AutoMapper.Mapper.Map<User>(user);
            newUser.CreatedAt = DateTime.Now;
            _userRpo.Create(newUser);
            _uow.Commit();

            return newUser.Id;
        }

        public bool UpdatetUser(int id, UserInput user)
        {
            var retrievedUser = _userRpo.GetWhere(w => w.Id == id).FirstOrDefault();
            if (retrievedUser == null)
                return false;

            retrievedUser.Address = user.Address;
            retrievedUser.Name = user.Name;
            retrievedUser.UpdatedAt = DateTime.Now;
            _userRpo.Edit(retrievedUser);
            _uow.Commit();

            return true;
        }

        public bool DeleteUser(int id)
        {
            var result = _userRpo.RemoveWhere(r => r.Id == 1);
            if (!result) return false;
            _uow.Commit();

            return true;
        }
    }
}
