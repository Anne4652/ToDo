using Microsoft.AspNetCore.Identity;

namespace ToDoApp.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user);
    }
}
