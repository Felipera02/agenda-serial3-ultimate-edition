using System.ComponentModel.DataAnnotations;

namespace AgendaApp.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome da categoria é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cor é obrigatória")]
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Cor deve estar no formato hexadecimal")]
        public string Color { get; set; } = "#3B82F6";
    }
}
