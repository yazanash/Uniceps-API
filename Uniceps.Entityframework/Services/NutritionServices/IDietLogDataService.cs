using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.Entityframework.Services.NutritionServices
{
    public interface IDietLogDataService
    {
        public Task<DietLog> UpsertAsync(DietLog dietLog);
        public Task<IEnumerable<DietLog>> GetAllByUserAsync(string? userId);
    }
}
