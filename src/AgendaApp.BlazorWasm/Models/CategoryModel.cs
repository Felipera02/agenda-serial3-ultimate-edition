using System.ComponentModel.DataAnnotations;

namespace AgendaApp.BlazorWasm.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome da categoria é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cor é obrigatória")]
        public string Color { get; set; } = "#3B82F6";
    }
}
