using Appointment.Business.Core.DTO;
using Appointment.Business.Services.Abstracts;
using Appointment.DataAccess.Entities;
using Appointment.DataAccess.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Business.Services.Concretes
{
    public class HelperService(IUnitOfWork unitOfWork) : IHelperService
    {
        public async Task FillAudit(IEnumerable<BaseDto> entities)
        {
            var userRepo = unitOfWork.GetRepository<User>();

            var allUserIds = entities
                .SelectMany(u => new[] { u.CreatedById, u.UpdatedById })
                .Where(id => id != null && id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var allUsers = await userRepo.GetAllNoPaginationAsync(
                select: s => new { s.Id, FullName = s.FirstName + " " + s.LastName },
                predicate: u => allUserIds.Contains(u.Id),
                enableTracking: false
            );

            var userLookup = allUsers.ToDictionary(u => u.Id, u => u.FullName);

            foreach (var entity in entities)
            {
                entity.CreatedBy = userLookup.TryGetValue(entity.CreatedById, out var createdByName) ? createdByName : string.Empty;
                entity.UpdatedBy = entity.UpdatedById.HasValue && userLookup.TryGetValue(entity.UpdatedById.Value, out var updatedByName) ? updatedByName : null;
            }
        }

        public async Task FillAudit(BaseDto entity)
        {
            var userRepo = unitOfWork.GetRepository<User>();

            entity.CreatedBy = await userRepo.GetAsync(s => $"{s.FirstName} {s.LastName}", p => p.Id == entity.CreatedById, enableTracking: false);

            if (entity.UpdatedById != null)
                entity.UpdatedBy = await userRepo.GetAsync(s => $"{s.FirstName} {s.LastName}", p => p.Id == entity.UpdatedById, enableTracking: false);
        }
    }
}
