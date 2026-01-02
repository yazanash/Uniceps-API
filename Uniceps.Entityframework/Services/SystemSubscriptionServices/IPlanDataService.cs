using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.Services.SystemSubscriptionServices
{
    public interface IPlanDataService
    {
        Task<PlanModel> Create(PlanModel entity);
        Task<bool> Delete(Guid id);
        Task<PlanModel> Get(Guid id);
        Task<IEnumerable<PlanModel>> GetPlansForApp(int productId);
        Task<PlanItem> GetItemById(int id);
        Task<PlanModel> Update(PlanModel entity);
    }
}
