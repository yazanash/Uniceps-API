using Microsoft.Identity.Client;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using static System.Net.WebRequestMethods;

namespace Uniceps.app.Services.TesterServices
{
    public class BypassService : IBypassService
    {
        private readonly IConfiguration _config;
        public BypassService(IConfiguration config) => _config = config;

        public bool IsTester(string email)
        {
            var isEnabled = _config.GetValue<bool>("InitialSetup:EnableBypassForTesters");
            if (!isEnabled) return false;
            var testers = _config.GetSection("InitialSetup:Testers").Get<List<string>>();
            return testers != null && testers.Contains(email);
        }

        public bool IsValidTester(string email, string otp)
        {
            var isEnabled = _config.GetValue<bool>("InitialSetup:EnableBypassForTesters");
            if (!isEnabled) return false;

            var testers = _config.GetSection("InitialSetup:Testers").Get<List<string>>();
            var staticOtp = _config["InitialSetup:StaticTesterOTP"];
            return testers != null && testers.Contains(email) && otp == staticOtp;
        }
        public SystemSubscriptionDto? GetSubscriptionForTester()
        {
            var isEnabled = _config.GetValue<bool>("InitialSetup:EnableBypassForTesters");
            if (!isEnabled) return null;
            return new SystemSubscriptionDto()
            {
                Id =Guid.NewGuid(),
                Price =0,
                Plan = "VIP For Tester",
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddMonths(1),
                IsActive =true,
                IsGift = true,
            };
        }
    }
}
