using FluentValidation;
using MediatR;
using MyService.Containers;
using MyService.Models;
using SC.Internship.Common.Exceptions;
namespace MyOwnServicw.Features.GiveTicket
{
    public class GiveTicketCommand : IRequest<Ticket>
    {

        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public class GiveTicketCommandHandler : IRequestHandler<GiveTicketCommand, Ticket>
        {
            private readonly DataEvent _dataevent;
            private readonly DataImage _images;
            public readonly DataSpace _spaces;

            public GiveTicketCommandHandler(DataEvent events, DataImage images, DataSpace space)
            {
                this._spaces = space ?? throw new ArgumentNullException(nameof(space));
                this._dataevent = events ?? throw new ArgumentNullException(nameof(events));
                this._images = images ?? throw new ArgumentNullException(nameof(images));
            }

            public async Task<Ticket> Handle(GiveTicketCommand command, CancellationToken cancellationToken)
            {
                int index = await this._dataevent.GetIdOfElem(command.EventId);
                if (index == -1) throw new ScException("Такого меропрятия не существет");

                Ticket? tick =  await this._dataevent.GiveTicket(command.UserId, index);
                if (tick is null) throw new ScException("Лишнего билета нет");
                return tick;
            }


        }
        public class GiveTicketCommandValidator : AbstractValidator<GiveTicketCommand>
        {
            public GiveTicketCommandValidator(DataImage images, DataSpace space)
            {


                RuleFor(c => c.EventId).NotEmpty().WithMessage("EventId: EventId не существет");
                RuleFor(c => c.UserId).NotEmpty().WithMessage("UserId: EventId не существет");
            }




        }

    }
}
