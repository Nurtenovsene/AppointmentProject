
namespace Appointment.DataAccess.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

    }
}
