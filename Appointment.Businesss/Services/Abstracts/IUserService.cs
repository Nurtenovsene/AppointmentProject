using Appointment.Business.Core.Common;
using Appointment.Business.DTOs.User;
using TourOperator.Business.Core.Results;

namespace Appointment.Business.Services.Abstracts
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetByIdAsync(Guid Id);
        Task<Result<List<UserDto>>> GetAllAsync();
        Task<Result> AddUserAsync(AddUserDto addUserDto);
        Task<Result> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<Result> DeleteUserAsync(DeleteDto deleteDto);
    }
}
