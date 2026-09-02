using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.NutritionSystem;
using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.Entityframework.Services.RoutineServices
{
    public interface IRoutineTemplateDataService
    {
        public Task<RoutineTemplate> CreateAsync(RoutineTemplate entity);
        public Task<RoutineTemplate> UpdateAsync(RoutineTemplate entity);
        public Task<bool> DeleteAsync(Guid id);
        public Task<bool> DeactiveAsync(Guid id);
        public Task<IEnumerable<RoutineTemplate>> GetAllByQuery(TargetGender? targetGender,TargetLanguage? targetLanguage);
        public Task<RoutineTemplate> Get(Guid id);
    }
}
