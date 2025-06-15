using Microsoft.AspNetCore.Identity;
using Uniceps.app.DTOs.AuthenticationDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Profile;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Uniceps.app.Services
{
    public class DataMigrationService
    {
        private readonly IDataService<NormalProfile> _profileDataService;
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly MongoDbService _mongoDbService;

        public DataMigrationService(IDataService<NormalProfile> profileDataService, AppDbContext appDbContext, UserManager<AppUser> userManager, MongoDbService mongoDbService)
        {
            _profileDataService = profileDataService;
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mongoDbService = mongoDbService;
        }

        public async Task MigrateData()
        {
            List<MongoUserDto> oldUsers = _mongoDbService.GetUsers();
            List<MongoProfileDto> oldProfiles = _mongoDbService.GetProfiles();
            foreach(MongoUserDto userDto in oldUsers)
            {
                AppUser newUser = new()
                {
                    Email = userDto.Email,
                    UserName = userDto.Email!.Split('@')[0],
                    UserType = UserType.Normal
                };
                IdentityResult result = await _userManager.CreateAsync(newUser);
                if(result.Succeeded)
                {
                    MongoProfileDto? oldProfile = oldProfiles.FirstOrDefault(x => x.MongoId == userDto.MongoId);
                    if (oldProfile != null)
                    {
                        NormalProfile normalProfile = new NormalProfile();
                        normalProfile.Name = oldProfile.Name;
                        normalProfile.Phone = oldProfile.Phone;
                        normalProfile.Gender = oldProfile.GenderMale=="True"? GenderType.Male:GenderType.Female;
                        normalProfile.UserId = newUser.Id;
                        await _profileDataService.Create(normalProfile);
                    }
                }
            }
        }
    }
}
