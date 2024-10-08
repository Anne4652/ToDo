using System.Security.Claims;
using ToDoApp.Interfaces.Services;

namespace ToDoApp.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User is not authenticated.");
            }
            return userId;
        }

        public ClaimsPrincipal GetUserClaimsPrincipal()
        {
            return _httpContextAccessor.HttpContext?.User ?? throw new Exception("User is not authenticated.");
        }
    }
}
