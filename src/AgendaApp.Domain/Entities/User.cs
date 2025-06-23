using Microsoft.AspNetCore.Identity;

namespace AgendaApp.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
