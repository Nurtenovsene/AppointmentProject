using Appointment.Business.Core.Common;
using Appointment.Business.Core.Results;
using Appointment.Business.DTOs.User;
using Appointment.Business.Services.Abstracts;
using Appointment.DataAccess.Entities;
using Appointment.DataAccess.Repositories;
using Appointment.DataAccess.UnitOfWorks;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourOperator.Business.Core.Results;

namespace Appointment.Business.Services.Concretes
{
    public class UserService(IUnitOfWork unitOfWork, IHelperService helperService) : IUserService
    {
        public async Task<Result<List<UserDto>>> GetAllAsync()
        {
            var userRepo = unitOfWork.GetRepository<User>();
            var users = await userRepo.GetAllAsync(
                select: p => new UserDto
                {
                    Id = p.Id,
                    Name = p.FirstName + " " + p.LastName,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    CreatedById = p.CreatedBy,
                    UpdatedById = p.UpdatedBy
                },
                orderBy: p => p.OrderBy(Route => Route.FirstName),
                enableTracking: false);

            await helperService.FillAudit(users);
            var userCount = await userRepo.CountAsync();
            return Result<List<UserDto>>.Success(users, userCount);
        }
        public async Task<Result<UserDto>> GetByIdAsync(Guid Id)
        {
            var UserRepo = unitOfWork.GetRepository<User>();
            var User = await UserRepo.GetAsync(
                select: p => new UserDto
                {
                    Id = p.Id,
                    Name = p.FirstName + " " + p.LastName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    CreatedById = p.CreatedBy,
                    UpdatedById = p.UpdatedBy
                },
                predicate: p => p.Id == Id,
                enableTracking: false);
            await helperService.FillAudit(User);
            return Result<UserDto>.Success(User);
        }

        public async Task<Result> AddUserAsync(AddUserDto addUserDto)
        {
            //same user control
            var userRepo = unitOfWork.GetRepository<User>();
            var existingUserWithEmail = await userRepo.GetAsync(
               u => (u.Email == addUserDto.Email || u.PhoneNumber == addUserDto.PhoneNumber) &&
                    !u.IsDeleted, enableTracking: false);

            if (existingUserWithEmail != null)
            {
                return Error.UserAlreadyExisting;
            }
            var user = addUserDto.Adapt<User>();
            user.CreateDate = DateTime.Now;
            await userRepo.AddAsync(user);
            await unitOfWork.SaveAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(DeleteDto deleteDto)
        {
            var UserRepo = unitOfWork.GetRepository<User>();
            var appoinmentRepo = unitOfWork.GetRepository<Appointments>();
            var user = await UserRepo.GetAsync(p => p.Id == deleteDto.Id, enableTracking: true);

            if (user == null) return Error.UserNotExist;
            //active appointment check
            bool hasAppoinments = await appoinmentRepo.AnyAsync(r => r.UserId == deleteDto.Id && !r.IsDeleted);
            if (hasAppoinments)
            {
                return Error.UserHasActiveAppoinment;
            }

            user.UpdateDate = DateTime.Now;
            user.IsDeleted = true;

            await unitOfWork.SaveAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await unitOfWork
                .GetRepository<User>()
                .GetAsync(p => p.Id == updateUserDto.Id, enableTracking: true);

            if (user == null)
                return Error.UserNotExist;

            var existingUserWithEmail = await unitOfWork
                .GetRepository<User>()
                .GetAsync(p => p.Email == updateUserDto.Email && !p.IsDeleted && p.Id != updateUserDto.Id, enableTracking: false);

            if (existingUserWithEmail != null)
            {
                return Error.UserNotExist;
            }
            user.UpdateDate = DateTime.Now;
            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            await unitOfWork.SaveAsync();

            return Result.Success();
        }

    }
}
