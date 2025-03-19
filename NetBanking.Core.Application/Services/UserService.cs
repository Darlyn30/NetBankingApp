using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;

        }
        public async Task<UserViewModel> GetById(string id)
        {
            UserDTO userDTO = await _accountService.FindByIdAsync(id);

            UserViewModel vm = _mapper.Map<UserViewModel>(userDTO);

            return vm;
        }

        public async Task<UserViewModel> GetByUsername(string username)
        {
            UserDTO userDTO = await _accountService.FindByUsernameAsync(username);

            UserViewModel vm = _mapper.Map<UserViewModel>(userDTO);

            return vm;
        }

        public async Task<SaveUserViewModel> GetUpdateUserAsync(string username)
        {
            UserDTO userDTO = await _accountService.FindByUsernameAsync(username);

            SaveUserViewModel userVm = new SaveUserViewModel()
            {
                Id = userDTO.Id,
                Username = userDTO.Username,
                Name = userDTO.Name,
                LastName = userDTO.LastName,
                IdentificationNumber = userDTO.IdentificationNumber,
                Email = userDTO.Email,
                Role = userDTO.Role == Roles.Admin.ToString() ? (int)Roles.Admin : (int)Roles.Client,
            };

            return userVm;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);

            if (vm.Role == (int)Roles.Admin)
            {
                registerRequest.Role = Roles.Admin.ToString();
            }
            else if (vm.Role == (int)Roles.Client)
            {
                registerRequest.Role = Roles.Client.ToString();
            }

            return await _accountService.RegisterUserAsync(registerRequest);
        }

        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        public async Task<Responses> UpdateUserAsync(SaveUserViewModel vm)
        {
            UpdateUserRequest updateRequest = _mapper.Map<UpdateUserRequest>(vm);

            return await _accountService.UpdateUserAsync(updateRequest);
        }
    }
}
