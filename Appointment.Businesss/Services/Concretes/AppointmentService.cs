using Appointment.Business.Core.Common;
using Appointment.Business.Core.Results;
using Appointment.Business.DTOs.Appointment;
using Appointment.Business.DTOs.User;
using Appointment.Business.Services.Abstracts;
using Appointment.DataAccess.Entities;
using Appointment.DataAccess.UnitOfWorks;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourOperator.Business.Core.Results;
using Microsoft.EntityFrameworkCore;


namespace Appointment.Business.Services.Concretes
{
    public class AppointmentService(IUnitOfWork unitOfWork, IHelperService helperService) : IAppointmentService
    {
        public async Task<Result<object>> GetAllAsync()
        {
            var appointmentRepo = unitOfWork.GetRepository<Appointments>();

            var appointments = await appointmentRepo.GetAllAsync(
                select: p => new AppointmentDto
                {
                    Id = p.Id,
                    Date = p.Date,
                    StartTime = p.StartTime,
                    EndTime = p.EndTime,
                    Description = p.Description,
                    User = new UserDto
                    {
                        Id = p.User.Id,
                        Name = p.User.FirstName + " " + p.User.LastName,
                        Email = p.User.Email,
                        PhoneNumber = p.User.PhoneNumber
                    }
                },
                orderBy: p => p.OrderBy(a => a.Date),
                enableTracking: false
            );

            await helperService.FillAudit(appointments);

            return Result<object>.Success(new { date = DateTime.Now.ToString("yyyy-MM-dd"), appointments });
        }


        public async Task<Result<AppointmentDto>> GetByIdAsync(Guid Id)
        {
            var appointmentRepo = unitOfWork.GetRepository<DataAccess.Entities.Appointments>();

            var appointment = await appointmentRepo.GetAsync(
                select: p => new AppointmentDto
                {
                    Id = p.Id,
                    Date = p.Date,
                    StartTime = p.StartTime,
                    EndTime = p.EndTime,
                    Description = p.Description,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    CreatedById = p.CreatedBy,
                    UpdatedById = p.UpdatedBy,
                    User = new UserDto
                    {
                        Id = p.User.Id,
                        Name = p.User.FirstName + " " + p.User.LastName,
                        Email = p.User.Email,
                        PhoneNumber = p.User.PhoneNumber
                    }
                },
                predicate: p => p.Id == Id,
                enableTracking: false);


            if (appointment == null)
            {
                return Error.Appointmentnotfound;
            }

            await helperService.FillAudit(appointment);

            return Result<AppointmentDto>.Success(appointment);
        }


        public async Task<Result> AddAppoinmentAsync(AddAppointmentDto addAppointmenDto)
        {
            var startHour = TimeSpan.FromHours(9);
            var endHour = TimeSpan.FromHours(18);
            var minDuration = TimeSpan.FromMinutes(30);
            var maxDuration = TimeSpan.FromHours(2);
            var today = DateTime.UtcNow.Date;
            var appointmentDuration = addAppointmenDto.EndTime - addAppointmenDto.StartTime;
            //working hours control
            if (addAppointmenDto.StartTime < startHour || addAppointmenDto.StartTime >= endHour)
            {
                return Error.InvalidStartTime;
            }
            if (addAppointmenDto.EndTime <= startHour || addAppointmenDto.EndTime > endHour)
            {
                return Error.InvalidEndTime;
            }
            //min-max appointment duration
            if (appointmentDuration < minDuration || appointmentDuration > maxDuration)
            {
                return Error.InvalidDuration;
            }

            var appointmentRepo = unitOfWork.GetRepository<Appointments>();

            var appointmentCount = await appointmentRepo.CountAsync(a =>
            a.UserId == addAppointmenDto.UserId &&
           !a.IsDeleted &&
            a.CreateDate.Date == today);
           // max number of appointments
            if (appointmentCount >= 2)
            {
                return Error.MaxDailyAppointmentsReached;
            }

            var conflictCount = await appointmentRepo.CountAsync(a =>
           a.UserId == addAppointmenDto.UserId &&
           !a.IsDeleted &&
           a.StartTime < addAppointmenDto.EndTime &&
           a.EndTime > addAppointmenDto.StartTime);

            //conflicting appointments
            if (conflictCount > 0)
            {
                return Error.AppointmentTimeConflict;
            }

            var appointment = addAppointmenDto.Adapt<DataAccess.Entities.Appointments>();
            appointment.Id = Guid.NewGuid();
            appointment.CreateDate = DateTime.UtcNow;
            appointment.UpdateDate = null;

            await appointmentRepo.AddAsync(appointment);
            await unitOfWork.SaveAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteAppoinmentAsync(DeleteDto deleteDto)
        {
            var appointmentRepo = unitOfWork.GetRepository<DataAccess.Entities.Appointments>();

            var appointment = await appointmentRepo.GetAsync(p => p.Id == deleteDto.Id, enableTracking: true);

            if (appointment == null) return Error.UserNotExist;
            appointment.UpdateDate = DateTime.Now;
            appointment.IsDeleted = true;

            await unitOfWork.SaveAsync();

            return Result.Success();
        }


        public async Task<Result> UpdateAppoinmentAsync(UpdateAppointmentDto updateAppointmentDto)
        {
            var appointment = await unitOfWork
                .GetRepository<DataAccess.Entities.Appointments>()
                .GetAsync(p => p.Id == updateAppointmentDto.Id, enableTracking: true);

            if (appointment == null)
                return Error.UserNotExist;


            appointment.UpdateDate = DateTime.Now;
            appointment.Date = updateAppointmentDto.Date;
            appointment.StartTime = updateAppointmentDto.StartTime;
            appointment.EndTime = updateAppointmentDto.EndTime;
            appointment.Description = updateAppointmentDto.Description;

            await unitOfWork.SaveAsync();

            return Result.Success();
        }

    }
}
