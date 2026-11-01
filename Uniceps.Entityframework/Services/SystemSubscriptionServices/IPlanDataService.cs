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
        Task<PlanItem> Create(PlanItem entity);
        Task<bool> Delete(int id);
        Task<PlanItem> Get(int id);
        Task<IEnumerable<PlanItem>> GetPlansForApp(int productId);
        Task<PlanItem> Update(PlanItem entity);
    }
}
