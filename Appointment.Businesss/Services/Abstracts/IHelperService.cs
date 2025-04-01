using Appointment.Business.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Business.Services.Abstracts
{
    public interface IHelperService
    {
        Task FillAudit(IEnumerable<BaseDto> entities);
        Task FillAudit(BaseDto entity);
    }

}
