using FluentValidation;
using MediatR;
using MyService.Models;

using MyService.Containers;
using SC.Internship.Common.Exceptions;

namespace MyService.Features.TicketCheck
{
    
        public class CheckTicketCommand : IRequest<Ticket?>
        {

            public Guid EventId { get; set; }
            public Guid UserId { get; set; }

            public class CheckTicketCommandHandler : IRequestHandler<CheckTicketCommand, Ticket?>
            {
                private readonly DataEvent _dataevent;
                private readonly DataImage _images;
                public readonly DataSpace _spaces;

                public CheckTicketCommandHandler(DataEvent events, DataImage images, DataSpace space)
                {
                    this._spaces = space ?? throw new ArgumentNullException(nameof(space));
                    this._dataevent = events ?? throw new ArgumentNullException(nameof(events));
                    this._images = images ?? throw new ArgumentNullException(nameof(images));
                }

                public async Task<Ticket?> Handle(CheckTicketCommand command, CancellationToken cancellationToken)
                {
                    int index = await this._dataevent.GetIdOfElem(command.EventId);
                    if (index == -1) throw new ScException("Такого меропрятия не существет");

                    return await this._dataevent.CheckTicket(command.UserId, index);
            }


            }
            public class CheckTicketCommandValidator : AbstractValidator<CheckTicketCommand>
            {
                public CheckTicketCommandValidator(DataImage images, DataSpace space)
                {


                    RuleFor(c => c.EventId).NotEmpty();
                    RuleFor(c => c.UserId).NotEmpty();
            }




            }

        }
    
}
