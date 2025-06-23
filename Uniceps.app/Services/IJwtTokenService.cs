using Uniceps.app.DTOs;
using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.app.Services
{
    public interface IJwtTokenService
    {
        JwtTokenResult GenerateToken(AppUser user, IList<string> roles);
    }
}
