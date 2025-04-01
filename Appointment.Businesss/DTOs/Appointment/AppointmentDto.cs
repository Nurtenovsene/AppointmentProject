using Appointment.Business.Core.DTO;
using Appointment.Business.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Appointment.Business.DTOs.Appointment
{
    public class AppointmentDto : BaseDto
    {
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string? Description { get; set; }
        public UserDto User { get; set; }
        [JsonIgnore]
        public string StartTimeFormatted => StartTime.ToString(@"hh\:mm");

        [JsonIgnore]
        public string EndTimeFormatted => EndTime.ToString(@"hh\:mm");
    }
}
