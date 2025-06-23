using System.ComponentModel.DataAnnotations;

namespace AgendaApp.Application.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data/hora de início é obrigatória")]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "Data/hora de fim é obrigatória")]
        public DateTime EndDateTime { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;
    }
}
