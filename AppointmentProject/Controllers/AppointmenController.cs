using Appointment.Business.Core.Common;
using Appointment.Business.DTOs.Appointment;
using Appointment.Business.DTOs.User;
using Appointment.Business.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AppoinmentController(IAppointmentService _appointmentService) : BaseController
{
    [HttpGet("GetAll")]

    public async Task<IActionResult> GetAll()
    {
        var result = await _appointmentService.GetAllAsync();
        return Ok(result);
    }
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _appointmentService.GetByIdAsync(id);
        return Ok(result);
    }
    [HttpPost("Add")]

    public async Task<IActionResult> Add([FromBody] AddAppointmentDto addAppointmentDto)
    {
        var result = await _appointmentService.AddAppoinmentAsync(addAppointmentDto);
        return Ok(result);
    }
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteDto deleteDto)
    {
        var result = await _appointmentService.DeleteAppoinmentAsync(deleteDto);
        return Ok(result);
    }
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateAppointmentDto updateAppointmentDto)
    {
        var result = await _appointmentService.UpdateAppoinmentAsync(updateAppointmentDto);
        return Ok(result);
    }
}

