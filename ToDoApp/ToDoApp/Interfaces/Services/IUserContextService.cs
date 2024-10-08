using System.Security.Claims;

namespace ToDoApp.Interfaces.Services
{
    public interface IUserContextService
    {
        string GetUserId();
        ClaimsPrincipal GetUserClaimsPrincipal();
    }
}
