using Appointment.Business.Core.DTO;


namespace Appointment.Business.DTOs.User
{
    public class UserDto:BaseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
