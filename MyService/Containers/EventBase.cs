using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyService.Models;
using System;
using SC.Internship.Common.Exceptions;
using SC.Internship.Common.ScResult;
using Microsoft.Extensions.Logging;

namespace MyService.Containers
{
    public interface IData
    {
        public  Task<List<Event>?> GetEvents(DateTime begin, DateTime end);
        public  Task<Ticket?> CheckTicket(Guid userid, Event ev);
        public  Task<bool> CheckFree(Event ev);
        public  Task<Ticket?> GiveTicket(Guid userid, Event ev, int place);
      
        public  Task<List<Event>> GetAll() ;
        public  Task<Event?> GetOne(Guid id) ;
        public  Task UpdateAsync(Guid id, Event ev) ;
        public  Task<Event> AddTickets(int count, Guid id, Event ev, List<int> numbers);
       
        public  Task RemoveAsync(Guid id) ;
        public  Task<int> GetNumberTicket(Ticket tick);

        public  Task<Event> AddEvent(Event ev);


    }
    public class DataService : IData
    {
        private readonly IMongoCollection<Event> events;

        public DataService(
            IOptions<DataOptions> bookStoreDatabaseSettings)
        {
            var connectionString = "mongodb://localhost:27017";
            var mongoUrl = new MongoUrl(connectionString);
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            events = mongoDatabase.GetCollection<Event>(
                "Events");
        }
        public async Task<List<Event>?> GetEvents(DateTime begin, DateTime end)
        {

            return await this.events.Find(p => (p.Begin >= begin) && (p.Begin <= end)).ToListAsync();

        }
        public async Task<Ticket?> CheckTicket(Guid userid, Event ev)
        {
            return await Task.Run(() =>
            {
                return ev.Tickets.FirstOrDefault(p => p.Id == userid);
            });

        }
        public async Task<bool> CheckFree(Event ev)
        {
            return await Task.Run(() =>
            {
                return ev.Free;
            });

        }
        public async Task<Ticket?> GiveTicket(Guid userid, Event ev, int place)
        {

            int tick = await Task.Run(() =>
            {
                return ev.Tickets.FindIndex(p => (p.UserId
            == null) && (p.Place == place));
            });
            if (tick == -1) return null;
            ev.Tickets[tick].UserId = userid;
            await this.UpdateAsync(ev.Id, ev);
            return ev.Tickets[tick];
        }
        public async Task<List<Event>> GetAll() =>
       await events.Find(_ => true).ToListAsync();
        public async Task<Event?> GetOne(Guid id) =>
            await events.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task UpdateAsync(Guid id, Event ev) =>
        await events.ReplaceOneAsync(x => x.Id == id, ev);
        public async Task<Event> AddTickets(int count, Guid id,  Event ev , List<int> numbers)
        {
            for (int i = 0; i < count; i++)
            {
                int ind = await Task.Run(() =>
                {
                    return ev.Tickets.FindIndex(p => (p.UserId
            == null) && (p.Place == numbers[i]));

                        
                });
                if (ind == -1) throw new ScException("Билет с местом "+ Convert.ToString(numbers[i]) + " уже существует");
                await Task.Run(() =>
                {
                    ev.Tickets.Add(new Ticket
                    {
                        Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa" + Convert.ToString(ev.Tickets.Count)),
                        Place = numbers[i]
                    }

                        );
                });
                
            }
            await this.UpdateAsync( id, ev);
            return ev;
        }
        public async Task RemoveAsync(Guid id) =>
            await events.DeleteOneAsync(x => x.Id == id);
        public async Task<int> GetNumberTicket(Ticket tick)
        {
            return await Task.Run(() =>
            {
                return tick.Place;
            });

        }
        public async Task<Event> AddEvent( Event ev)
        {
            var t = await events.Find(_ => true).ToListAsync();
            await events.InsertOneAsync(ev);
            return ev;
        }


    }
}
