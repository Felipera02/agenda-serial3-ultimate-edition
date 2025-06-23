using AgendaApp.Application.DTOs;
using AgendaApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgendaApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentsController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var appointments = await _appointmentService.GetUserAppointmentsAsync(userId, startDate, endDate);
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (appointmentDto.StartDateTime >= appointmentDto.EndDateTime)
                return BadRequest("Data/hora de início deve ser anterior à data/hora de fim");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                var appointment = await _appointmentService.CreateAppointmentAsync(appointmentDto, userId);
                return CreatedAtAction(nameof(GetAppointments), new { id = appointment.Id }, appointment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != appointmentDto.Id)
                return BadRequest("ID do compromisso não confere");

            if (appointmentDto.StartDateTime >= appointmentDto.EndDateTime)
                return BadRequest("Data/hora de início deve ser anterior à data/hora de fim");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                var appointment = await _appointmentService.UpdateAppointmentAsync(appointmentDto, userId);
                return Ok(appointment);
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                await _appointmentService.DeleteAppointmentAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
