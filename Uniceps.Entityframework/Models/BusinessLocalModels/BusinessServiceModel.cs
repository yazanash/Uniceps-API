using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.BusinessLocalModels
{
    public class BusinessServiceModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public Durationtype DurationType { get; set; }
        public int Duration { get; set; }
        public int SessionCount { get; set; }
        public string? BusinessId {  get; set; }
        public string? TrainerId { get; set; }
    }
    public enum Durationtype
    {
        Daily,
        Weekly,
        monthly
    }
}
