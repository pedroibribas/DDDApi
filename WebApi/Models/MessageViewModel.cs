namespace WebApi.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
    }
}
