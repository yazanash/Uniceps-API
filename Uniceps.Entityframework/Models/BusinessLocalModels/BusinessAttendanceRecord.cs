using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.BusinessLocalModels
{
    public class BusinessAttendanceRecord:EntityBase
    {
        public Guid PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public PlayerModel? Player { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? Note { get; set; } 
        public string? BusinessId { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
