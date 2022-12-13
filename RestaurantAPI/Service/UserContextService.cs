using System.Security.Claims;

namespace RestaurantAPI.Service
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _hhtContextAccessor;

        public UserContextService(IHttpContextAccessor hhtContextAccessor)
        {
            _hhtContextAccessor = hhtContextAccessor;
        }

        public ClaimsPrincipal User => _hhtContextAccessor.HttpContext?.User;
        public int? GetUserId => User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
