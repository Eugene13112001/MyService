namespace MyService.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public int Place { get; set; }


    }
}
