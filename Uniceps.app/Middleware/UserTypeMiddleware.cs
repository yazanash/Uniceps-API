namespace Uniceps.app.Middleware
{
    public class UserTypeMiddleware
    {
        private readonly RequestDelegate _next;

        public UserTypeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userType = context.User.FindFirst("userType")?.Value;

            if (!string.IsNullOrWhiteSpace(userType))
            {
                context.Items["UserType"] = userType;
            }

            await _next(context);
        }
    }
}
