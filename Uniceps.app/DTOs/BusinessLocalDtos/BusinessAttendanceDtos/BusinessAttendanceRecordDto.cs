namespace Uniceps.app.DTOs.BusinessLocalDtos.BusinessAttendanceDtos
{
    public class BusinessAttendanceRecordDto
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? Note { get; set; }
    }
}
