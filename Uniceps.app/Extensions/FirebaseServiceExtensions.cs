using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace Uniceps.app.Extensions
{
    public static class FirebaseServiceExtensions
    {
        public static IServiceCollection AddFirebaseAdmin(this IServiceCollection services, IConfiguration config)
        {
            var jsonSection = config.GetSection("Firebase:ServiceAccountJson").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            var rawJson = JsonConvert.SerializeObject(jsonSection);
            var credential = GoogleCredential.FromJson(rawJson);

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions { Credential = credential });
            }

            services.AddSingleton(FirebaseMessaging.DefaultInstance);
            return services;
        }
    }
}
