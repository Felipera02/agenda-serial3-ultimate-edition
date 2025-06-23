namespace AgendaApp.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual User User { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
