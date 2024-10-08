using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ToDoApp.Interfaces.Services;
using ToDoApp.Services.DTOs.Account;

namespace ToDoApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly ILogger<AccountService> _logger;
        public AccountService(UserManager<IdentityUser> userManager, ITokenService tokenService, SignInManager<IdentityUser> signInManager, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
            _logger = logger;
        }

        public async Task<NewUserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null)
            {
                throw new ArgumentException("Invalid username!");
            }

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                throw new ArgumentException("Username not found and/or password incorrect");
            }

            _logger.LogInformation("Login with username:", user.UserName);
            var userDto = new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };

            return userDto;
        }

        public async Task<NewUserDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new IdentityUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

            _logger.LogInformation("Registred user with username:", user.UserName);
            if (createdUser.Succeeded)
            {
                return new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                };
            }
            else
            {
                throw new ArgumentException("User not created!");
            }
        }
    }
}
