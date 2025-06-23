using AgendaApp.Application.DTOs;
using AgendaApp.Domain.Entities;
using AgendaApp.Infrastructure.Data;
using AgendaApp.Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Application.Services
{
    public class AppointmentService
    {
        private readonly GenericRepository<Appointment> _appointmentRepository;
        private readonly AppDbContext _context;

        public AppointmentService(GenericRepository<Appointment> appointmentRepository, AppDbContext context)
        {
            _appointmentRepository = appointmentRepository;
            _context = context;
        }

        public async Task<IEnumerable<AppointmentDto>> GetUserAppointmentsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Category)
                .Where(a => a.UserId == userId &&
                           a.StartDateTime >= startDate &&
                           a.StartDateTime <= endDate)
                .ToListAsync();

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                StartDateTime = a.StartDateTime,
                EndDateTime = a.EndDateTime,
                CategoryId = a.CategoryId,
                CategoryName = a.Category.Name,
                CategoryColor = a.Category.Color,
                IsCompleted = a.IsCompleted
            });
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointmentDto, string userId)
        {
            // Verificar se a categoria pertence ao usuário
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == appointmentDto.CategoryId && c.UserId == userId);

            if (!categoryExists)
                throw new UnauthorizedAccessException("Categoria não encontrada ou não pertence ao usuário");

            var appointment = new Appointment
            {
                Title = appointmentDto.Title,
                Description = appointmentDto.Description,
                StartDateTime = appointmentDto.StartDateTime,
                EndDateTime = appointmentDto.EndDateTime,
                CategoryId = appointmentDto.CategoryId,
                UserId = userId,
                IsCompleted = appointmentDto.IsCompleted
            };

            var createdAppointment = await _appointmentRepository.AddAsync(appointment);

            var category = await _context.Categories.FindAsync(appointmentDto.CategoryId);

            return new AppointmentDto
            {
                Id = createdAppointment.Id,
                Title = createdAppointment.Title,
                Description = createdAppointment.Description,
                StartDateTime = createdAppointment.StartDateTime,
                EndDateTime = createdAppointment.EndDateTime,
                CategoryId = createdAppointment.CategoryId,
                CategoryName = category?.Name ?? "",
                CategoryColor = category?.Color ?? "",
                IsCompleted = createdAppointment.IsCompleted
            };
        }

        public async Task<AppointmentDto> UpdateAppointmentAsync(AppointmentDto appointmentDto, string userId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == appointmentDto.Id && a.UserId == userId);

            if (appointment == null)
                throw new UnauthorizedAccessException("Compromisso não encontrado ou não pertence ao usuário");

            // Verificar se a nova categoria pertence ao usuário
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == appointmentDto.CategoryId && c.UserId == userId);

            if (!categoryExists)
                throw new UnauthorizedAccessException("Categoria não encontrada ou não pertence ao usuário");

            appointment.Title = appointmentDto.Title;
            appointment.Description = appointmentDto.Description;
            appointment.StartDateTime = appointmentDto.StartDateTime;
            appointment.EndDateTime = appointmentDto.EndDateTime;
            appointment.CategoryId = appointmentDto.CategoryId;
            appointment.IsCompleted = appointmentDto.IsCompleted;
            appointment.UpdatedAt = DateTime.UtcNow;

            var updatedAppointment = await _appointmentRepository.UpdateAsync(appointment);

            return new AppointmentDto
            {
                Id = updatedAppointment.Id,
                Title = updatedAppointment.Title,
                Description = updatedAppointment.Description,
                StartDateTime = updatedAppointment.StartDateTime,
                EndDateTime = updatedAppointment.EndDateTime,
                CategoryId = updatedAppointment.CategoryId,
                CategoryName = updatedAppointment.Category.Name,
                CategoryColor = updatedAppointment.Category.Color,
                IsCompleted = updatedAppointment.IsCompleted
            };
        }

        public async Task DeleteAppointmentAsync(int appointmentId, string userId)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId && a.UserId == userId);

            if (appointment == null)
                throw new UnauthorizedAccessException("Compromisso não encontrado ou não pertence ao usuário");

            await _appointmentRepository.DeleteAsync(appointment);
        }
    }
}
