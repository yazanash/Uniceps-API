using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.app.DTOs
{
    public class JwtTokenResult
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public UserType UserType { get; set; }
    }

}
