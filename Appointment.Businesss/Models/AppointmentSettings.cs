using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Business.Models
{
    public class AppointmentSettings
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int MinDurationMinutes { get; set; }
        public int MaxDurationMinutes { get; set; }
    }

}
