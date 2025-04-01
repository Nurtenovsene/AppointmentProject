using Appointment.Business.Core.Common;
using Appointment.Business.DTOs.Appointment;
using TourOperator.Business.Core.Results;

namespace Appointment.Business.Services.Abstracts
{
    public interface IAppointmentService
    {
        Task<Result<AppointmentDto>> GetByIdAsync(Guid Id);
        Task<Result<object>> GetAllAsync();
        Task<Result> AddAppoinmentAsync(AddAppointmentDto addUserDto);
        Task<Result> UpdateAppoinmentAsync(UpdateAppointmentDto updateUserDto);
        Task<Result> DeleteAppoinmentAsync(DeleteDto deleteDto);
    }
}

