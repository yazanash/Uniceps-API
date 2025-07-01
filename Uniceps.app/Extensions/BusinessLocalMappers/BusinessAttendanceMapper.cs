using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessAttendanceDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Extensions.BusinessLocalMappers
{
    public class BusinessAttendanceMapper : IMapperExtension<BusinessAttendanceRecord, BusinessAttendanceRecordDto, BusinessAttendanceRecordCreationDto>
    {
        public BusinessAttendanceRecord FromCreationDto(BusinessAttendanceRecordCreationDto data)
        {
            BusinessAttendanceRecord businessAttendance = new BusinessAttendanceRecord();
            businessAttendance. PlayerId = data.PlayerId;
            businessAttendance.CheckInTime = data.CheckInTime;
            businessAttendance.CheckOutTime = data.CheckOutTime;
            businessAttendance.Note = data.Note;
            return businessAttendance;
        }

        public BusinessAttendanceRecordDto ToDto(BusinessAttendanceRecord data)
        {
            BusinessAttendanceRecordDto businessAttendanceDto = new();
            businessAttendanceDto.Id = data.NID;
            businessAttendanceDto.PlayerId = data.PlayerId;
            businessAttendanceDto.CheckInTime = data.CheckInTime;
            businessAttendanceDto.CheckOutTime = data.CheckOutTime;
            businessAttendanceDto.Note = data.Note;
            return businessAttendanceDto;
        }
    }
}
