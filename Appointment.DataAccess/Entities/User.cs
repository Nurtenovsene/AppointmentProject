using System.ComponentModel.DataAnnotations;


namespace Appointment.DataAccess.Entities
{
    public class User : BaseEntity
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Appointments>? Appointments { get; set; }
    }
}
