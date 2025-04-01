using Appointment.Business.Models;
using Appointment.Business.Services.Abstracts;
using Appointment.Business.Services.Concretes;
using Appointment.DataAccess.Context;
using Appointment.DataAccess.Entities;
using Appointment.DataAccess.Repositories;
using Appointment.DataAccess.UnitOfWorks;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppointmentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentConnection")));


builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHelperService, HelperService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
