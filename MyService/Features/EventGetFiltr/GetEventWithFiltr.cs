using FluentValidation;
using MediatR;
using MyService.Containers;
using MyService.Features.EventGet;
using MyService.Models;

namespace MyService.Features.EventGetFiltr
{
    public class GetAllEventFiltrCommand : IRequest<List<Event>>
    {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public class GetAllEventFiltrCommandHandler : IRequestHandler<GetAllEventFiltrCommand, List<Event>>
        {
            private readonly DataEvent _dataevent;

            public GetAllEventFiltrCommandHandler(DataEvent events)
            {
                _dataevent = events ?? throw new ArgumentNullException(nameof(events));
            }

            public async Task<List<Event>> Handle(GetAllEventFiltrCommand command, CancellationToken cancellationToken)
            {
                return await this._dataevent.GetEvents(command.Begin, command.End);


            }


        }
        public class GetAllEventFiltrCommandValidator : AbstractValidator<GetAllEventFiltrCommand>
        {
            public GetAllEventFiltrCommandValidator()
            {

                RuleFor(c => c.Begin).NotEmpty();
                RuleFor(c => c.End).NotEmpty();
                RuleFor(c => this.DateCheck(c.Begin, c.End)).Equal(true)
                                    .WithMessage("Date: Дата конца дожна быть больше даты начала");
            }

            private bool DateCheck(DateTime? begin, DateTime? end)
            {
                if (begin is null) return false;
                if (end is null) return false;
                if (begin <= end) return true;
                return false;
            }

        }
    }
}
