namespace MyService.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public Guid ImageId { get; set; }

        public Guid SpaceId { get; set; }

        public List<Ticket> Tickets { get; set; }

        public bool Free { get; set; }


    }
}
