using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.User;
using NetBanking.Infrastructure.Identity.Entities;


namespace NetBanking.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region GetActiveUsersCountAsync & GetInactiveUsersCountAsync
        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _userManager.Users.Where(user => user.IsActive == true).CountAsync();
        }

        public async Task<int> GetInactiveUsersCountAsync()
        {
            return await _userManager.Users.Where(user => user.IsActive == false).CountAsync();
        }
        #endregion

        #region Login & Logout
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No accounts registered with the username: {request.UserName}.";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.UserName}.";
                return response;
            }

            if (!user.IsActive)
            {
                response.HasError = true;
                response.Error = $"Account disactive for {request.UserName}, please contact the administrator.";
                return response;
            }

            response.Id = user.Id;
            response.Name = user.Name;
            response.LastName = user.LastName;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.IdentificationNumber = user.IdentificationNumber;
            response.IsActive = user.IsActive;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion

        #region Register
        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
                return response;
            }

            var user = new ApplicationUser
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                IdentificationNumber = request.IdentificationNumber,
                IsActive = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                if (request.Role == Roles.Admin.ToString())
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
                else if (request.Role == Roles.Client.ToString())
                {
                    await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
                }
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred while trying to register the user.";
                return response;
            }

            return response;
        }
        #endregion

        #region GetAllUserAsync
        public async Task<List<UserDTO>> GetAllUserAsync()
        {
            var userList = await _userManager.Users.ToListAsync();

            var userDTOList = userList.Select(user => new UserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                LastName = user.LastName,
                IdentificationNumber = user.IdentificationNumber,
                Email = user.Email,
                IsActive = user.IsActive
            }).ToList();

            foreach (var userDTO in userDTOList)
            {
                var user = await _userManager.FindByIdAsync(userDTO.Id);

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDTO.Role = rolesList.ToList()[0];
            }

            return userDTOList;
        }
        #endregion

        #region FindByUsernameAsync & FindByIdAsync
        public async Task<UserDTO> FindByUsernameAsync(string username)
        {
            UserDTO userDTO = new();

            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                userDTO.Id = user.Id;
                userDTO.Username = user.UserName;
                userDTO.Name = user.Name;
                userDTO.LastName = user.LastName;
                userDTO.IdentificationNumber = user.IdentificationNumber;
                userDTO.Email = user.Email;
                userDTO.IsActive = user.IsActive;

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDTO.Role = rolesList.ToList()[0];

                return userDTO;
            }

            return null;
        }

        public async Task<UserDTO> FindByIdAsync(string id)
        {
            UserDTO userDTO = new();

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                userDTO.Id = user.Id;
                userDTO.Username = user.UserName;
                userDTO.Name = user.Name;
                userDTO.LastName = user.LastName;
                userDTO.IdentificationNumber = user.IdentificationNumber;
                userDTO.IsActive = user.IsActive;
                userDTO.Email = user.Email;

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDTO.Role = rolesList.ToList()[0];

                return userDTO;
            }

            return null;
        }
        #endregion

        #region Update
        public async Task<Responses> UpdateUserAsync(UpdateUserRequest request)
        {
            Responses response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByIdAsync(request.Id);
            user.Name = request.Name;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.IdentificationNumber = request.IdentificationNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (request.Password != null)
                {
                    // Update Password
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result = await _userManager.ResetPasswordAsync(user, token, request.Password);

                    if (!result.Succeeded)
                    {
                        response.HasError = true;
                        response.Error = $"An error has ocurred trying to update the password.";
                        return response;
                    }
                }
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error has ocurred trying to update the user {user.UserName}.";
                return response;
            }

            return response;
        }

        #endregion

        #region Active & Unactive 
        public async Task<Responses> UpdateUserStatusAsync(string username)
        {
            Responses response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"User: {username} not found.";
                return response;
            }

            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error has ocurred trying to update the status of the user: {username}.";
                return response;
            }

            return response;
        }
        #endregion
    }
}
