using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.Entityframework.Services
{
    public class OTPGenerateService(AppDbContext dbContext) : IOTPGenerateService<OTPModel>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<OTPModel> GenerateAsync(string email)
        {
            List<OTPModel> otps = await _dbContext.Set<OTPModel>().Where(x => x.Email == email).ToListAsync();
            _dbContext.RemoveRange(otps);
            OTPModel model = new OTPModel();
            model.Email = email;
            model.Otp = new Random().Next(111111, 999999);
            model.ExpireDate = DateTime.Now.AddMinutes(30);
            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<OTPModel?> VerifyAsync(string email, int otp)
        {
            OTPModel? oTPModel = await _dbContext.OTPModels.FirstOrDefaultAsync(x => x.Email == email);
            if (oTPModel != null && oTPModel.Otp == otp && oTPModel.ExpireDate.Subtract(DateTime.Now).TotalMinutes > 0)
            {
                _dbContext.OTPModels.Remove(oTPModel);
                await _dbContext.SaveChangesAsync();
                return oTPModel;
            }
            return null;
        }
    }
}
