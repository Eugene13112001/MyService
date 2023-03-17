using FluentValidation;
using MediatR;
using MyService.Containers;
using MyService.Models;

namespace MyService.Features.EventGetAll
{
    public class GetAllEventCommand : IRequest<List<Event>>
    {

        
        public class GetAllEventCommandHandler : IRequestHandler<GetAllEventCommand, List<Event>>
        {
            private readonly DataEvent _dataevent;

            public GetAllEventCommandHandler(DataEvent events)
            {
                _dataevent = events ?? throw new ArgumentNullException(nameof(events));
            }

            public async Task<List<Event>> Handle(GetAllEventCommand command, CancellationToken cancellationToken)
            {
                return await this._dataevent.GetAll();


            }
            
        }
    }
}
