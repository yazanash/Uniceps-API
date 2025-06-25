using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.DTOs.BusinessLocalDtos.BusinessServicesDtos
{
    public class BusinessServiceCreationDto
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public Durationtype DurationType { get; set; }
        public int Duration { get; set; }
        public int SessionCount { get; set; }
        public string? TrainerId { get; set; }
    }
}
