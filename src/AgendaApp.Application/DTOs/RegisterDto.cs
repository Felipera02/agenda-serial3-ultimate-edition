using System.ComponentModel.DataAnnotations;

namespace AgendaApp.Application.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("Password", ErrorMessage = "Senhas não coincidem")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
