using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.app.Helpers
{
    public static class HttpContextExtensions
    {
        public static bool IsBusinessUser(this HttpContext context)
       => context.Items["UserType"]?.ToString() == UserType.Business.ToString();

        public static bool IsPlayerUser(this HttpContext context)
            => context.Items["UserType"]?.ToString() == UserType.Normal.ToString();
        public static string GetLanguage(this HttpRequest request)
        {
            return request.Headers["Accept-Language"].ToString()
                .ToLower().StartsWith("ar") ? "ar" : "en";
        }
    }
}
