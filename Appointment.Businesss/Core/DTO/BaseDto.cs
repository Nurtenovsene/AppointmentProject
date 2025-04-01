using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Appointment.Business.Core.DTO
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdateDate { get; set; }

        [JsonIgnore]
        public Guid CreatedById { get; set; }
        [JsonIgnore]
        public Guid? UpdatedById { get; set; }
    }
}
