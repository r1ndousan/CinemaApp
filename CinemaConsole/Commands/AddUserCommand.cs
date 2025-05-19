using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;

namespace CinemaConsole.Commands
{
    public class AddUserCommand : ICommand
    {
        private readonly IClientRepository _clientRepo; // клиентские профили всё ещё тут
        private readonly IUserRepository _userRepo;
        private readonly User _user;

        public AddUserCommand(IUserRepository userRepo, User user)
        {
            _userRepo = userRepo;
            _user = user;
        }

        public async Task ExecuteAsync()
        {
            await _userRepo.AddUserAsync(_user);
        }
    }
}
