using FluentValidation;
using MediatR;
using MyService.Containers;
using MyService.Models;
using SC.Internship.Common;
using SC.Internship.Common.Exceptions;

namespace MyService.Features.TicketCreate
{
    public class AddTicketCommand : IRequest<bool>
    {


        public Guid EventId { get; set; }
        public int Count { get; set; }
        public List<int> Places { get; set; }

        public class AddTicketCommandHandler : IRequestHandler<AddTicketCommand, bool>
        {
            private readonly DataEvent _dataevent;
            private readonly DataImage _images;
            public readonly DataSpace _spaces;

            public AddTicketCommandHandler(DataEvent events, DataImage images, DataSpace space)
            {
                this._spaces = space ?? throw new ArgumentNullException(nameof(space));
                this._dataevent = events ?? throw new ArgumentNullException(nameof(events));
                this._images = images ?? throw new ArgumentNullException(nameof(images));
            }

            public async Task<bool> Handle(AddTicketCommand command, CancellationToken cancellationToken)
            {
                int index = await this._dataevent.GetIdOfElem(command.EventId);
                if (index == -1) throw new ScException("Такого события нет");
                bool ch = await this._dataevent.CheckFree(index);
                if (!ch) throw new ScException("Мест нет");
                return await this._dataevent.AddTickets(command.Count, index);
            }


        }
        public class AddProductCommandValidator : AbstractValidator<AddTicketCommand>
        {
            public AddProductCommandValidator(DataImage images, DataSpace space)
            {

               
                RuleFor(c => c.EventId).NotEmpty().NotEmpty().WithMessage("EventId: EventId не существет");
                RuleFor(c => c.Count).NotEmpty().WithMessage("EventId: Count не существет");
                RuleFor(c => (c.Count == c.Places.Count)).Equal(true)
                   .WithMessage("Count: Количество добавляемых мест должно быть равно длине массива");


            }

         

          
        }

    }
}
