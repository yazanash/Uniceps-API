using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.DTOs.BusinessLocalDtos.BusinessAttendanceDtos
{
    public class BusinessAttendanceRecordCreationDto
    {
        public Guid PlayerId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? Note { get; set; }
    }
}
