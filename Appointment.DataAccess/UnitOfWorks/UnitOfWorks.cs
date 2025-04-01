using Appointment.DataAccess.Context;
using Appointment.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.DataAccess.UnitOfWorks
{
    public class UnitOfWork(AppointmentDbContext dbContext) : IUnitOfWork
    {
        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        IRepository<T> IUnitOfWork.GetRepository<T>()
        {
            return new Repository<T>(dbContext);
        }
    }
}
