using Microsoft.AspNetCore.Mvc;
using ToDoApp.Services.DTOs.Account;

namespace ToDoApp.Interfaces.Services
{
    public interface IAccountService
    {
        Task<NewUserDto> LoginAsync(LoginDto loginDto);
        Task<NewUserDto> RegisterAsync(RegisterDto dto);
    }
}
