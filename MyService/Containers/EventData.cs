using System.Collections;
using MyService.Models;
namespace MyService.Containers
{
    public interface DataEvent
    {
       
        public Task<Event> AddEvent(Event ev, int count);
        public Task<Event> ChangeElement(int id, Event ev);

        public Task<bool> RemoveElement(Event ev);

        public Task<Event?> GetElementId(Guid id);

        public Task<int> GetIdOfElem(Guid id);
        public Task<List<Event>?> GetAll();
        public Task<bool> CheckFree(int index);
        public Task<List<Event>> GetEvents(DateTime begin , DateTime end);
        public Task<bool> AddTickets(int count, int index);
        public Task<Ticket?> CheckTicket(Guid userid, int index);
        public Task<Ticket?> GiveTicket(Guid userid, int index);

    }
    public class EventData : DataEvent
    {
        private int Id = 8;
        private int TicketId = 8;
        public Dictionary<Guid, int> eventsnumber = new Dictionary<Guid, int> {  { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), 0},
        { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"), 0},};
        


        public List<Event> events = new List<Event> {
                new Event { Id= new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "1" ,
                Description = "ff" , Begin = new DateTime(2022, 7, 2), End = new DateTime(2022, 8, 2),
                ImageId=  new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                 SpaceId=  new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                 Tickets = new List<Ticket>(),
                 Free = true
                },
                 new Event { Id= new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"), Name = "2" ,
                Description = "ff" , Begin = new DateTime(2022, 7, 2), End = new DateTime(2022, 8, 2),
                ImageId=  new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                 SpaceId=  new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                 Tickets = new List<Ticket>(),
                 Free = false
                }

        };


        public async Task<Event> AddEvent(Event ev, int count)
        {
            ev.Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa" + Convert.ToString(this.Id));
            this.Id += 1;
            await Task.Run(() => { this.events.Add(ev); });
            int index = this.events.Count - 1;
            await this.AddTickets(count, index);
            return ev;
        }
        public async Task<bool> RemoveElement(Event ev)
        {
           return await Task.Run(() => {return this.events.Remove(ev); });
        }
        public async Task<Event?> GetElementId(Guid id)
        {
            return await Task.Run(() => {return this.events.FirstOrDefault(p => p.Id == id); });
            
        }
        public async Task<Event> ChangeElement(int id, Event ev)
        {
            await Task.Run(() => { this.events[id] = ev; });
            return this.events[id];
        }
        public async Task<int> GetIdOfElem(Guid id)
        {
            return await Task.Run(() => { return this.events.FindIndex(p => p.Id == id); });
        }

        public async Task<List<Event>?> GetEvents(DateTime begin, DateTime end)
        {
            return await Task.Run(() => {
                return this.events.Where(p => (p.Begin >= begin) && (p.Begin <= end)).ToList();
            });
        }
        public async Task<List<Event>?> GetAll()
        {
            return await Task.Run(() => {
                return this.events.ToList();
            });
        }
        public async Task<bool> AddTickets(int count, int index)
        {
            for (int i = 0; i < count; i++)
            {
                await Task.Run(() =>
                {
                    this.events[index].Tickets.Add(new Ticket
                    {
                        Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa" + Convert.ToString(this.TicketId)),
                        Place = eventsnumber[this.events[index].Id]
                    }

                        );
                });
                eventsnumber[this.events[index].Id] += 1;
                TicketId += 1;
            }
            return true;
        }
        public async Task<Ticket?> CheckTicket(Guid userid, int index)
        {
            return await Task.Run(() =>
            {
                return this.events[index].Tickets.FirstOrDefault(p => p.UserId
            == userid);
            });
            
        }
        public async Task<bool> CheckFree(int index)
        {
            return await Task.Run(() =>
            {
                return this.events[index].Free;
            });

        }
        public async Task<Ticket?> GiveTicket(Guid userid, int index)
        {
            int tick =  await Task.Run(() =>
            {
                return this.events[index].Tickets.FindIndex(p => p.UserId
            == null);
            });
            if (tick == -1) return null;
            this.events[index].Tickets[tick].UserId = userid;
            return this.events[index].Tickets[tick];
        }

    }
}
