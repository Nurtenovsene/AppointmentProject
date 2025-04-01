using Appointment.Business.Core.Common;
using Appointment.Business.DTOs.User;
using Appointment.Business.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService _userService) : BaseController
{
    [HttpGet("GetAll")]

    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userService.GetByIdAsync(id);
        return Ok(result);
    }
    [HttpPost("Add")]

    public async Task<IActionResult> Add([FromBody] AddUserDto addUserDto)
    {
        var result = await _userService.AddUserAsync(addUserDto);
        return Ok(result);
    }
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteDto deleteDto)
    {
        var result = await _userService.DeleteUserAsync(deleteDto);
        return Ok(result);
    }
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto)
    {
        var result = await _userService.UpdateUserAsync(updateUserDto);
        return Ok(result);
    }
}

