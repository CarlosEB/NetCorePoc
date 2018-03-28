using System;
using System.Collections.Generic;
using System.Linq;
using NetCorePoc.Domain.Entities;
using NetCorePoc.Domain.Interfaces.Repositories;
using NetCorePoc.Domain.Interfaces.Service;
using NetCorePoc.Domain.Interfaces.UnitOfWork;

namespace NetCorePoc.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;

        public UserService(IUserRepository userRepository, IUnitOfWork uow)
        {
            _userRepository = userRepository;
            _uow = uow;
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetAll();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetWhere(w => w.Id == id).FirstOrDefault();            
        }

        public int InsertUser(User newUser)
        {            
            newUser.CreatedAt = DateTime.Now;
            _userRepository.Create(newUser);
            _uow.Commit();

            return newUser.Id;
        }

        public bool UpdatetUser(int id, User user)
        {
            var retrievedUser = _userRepository.GetWhere(w => w.Id == id).FirstOrDefault();
            if (retrievedUser == null)
                return false;

            retrievedUser.Address = user.Address;
            retrievedUser.Name = user.Name;
            retrievedUser.UpdatedAt = DateTime.Now;

            _userRepository.Edit(retrievedUser);
            _uow.Commit();

            return true;
        }

        public bool DeleteUser(int id)
        {
            var result = _userRepository.RemoveWhere(r => r.Id == 1);
            if (!result) return false;
            _uow.Commit();

            return true;
        }
    }
}
