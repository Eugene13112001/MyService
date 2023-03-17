using FluentValidation;
using MediatR;
using MyService.Containers;
using MyService.Models;

namespace MyService.Features.EventChange
{
    public class ChangeEventCommand : IRequest<Event?>
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public Guid ImageId { get; set; }

        public Guid SpaceId { get; set; }

        public bool Free { get; set; }
        public class ChangeEventCommandHandler : IRequestHandler<ChangeEventCommand, Event?>
        {
            private readonly DataEvent _dataevent;
            private readonly DataImage _images;
            public readonly DataSpace _spaces;

            public ChangeEventCommandHandler(DataEvent events, DataImage images, DataSpace space)
            {
                this._spaces = space ?? throw new ArgumentNullException(nameof(space));
                this._dataevent = events ?? throw new ArgumentNullException(nameof(events));
                this._images = images ?? throw new ArgumentNullException(nameof(images));
            }

            public async Task<Event?> Handle(ChangeEventCommand command, CancellationToken cancellationToken)
            {
                Event ev = new Event();
                ev.Id = command.Id;
                ev.Name = command.Name;
                ev.Free = command.Free;
                ev.Description = command.Description;
                ev.Begin = command.Begin;
                ev.End = command.End;
                ev.ImageId = command.ImageId;
                ev.SpaceId = command.SpaceId;
                int id = await this._dataevent.GetIdOfElem(ev.Id);
                if (id == -1) return null;
                await this._dataevent.ChangeElement(id, ev);
                return ev;
            }


        }
        public class ChangeProductCommandValidator : AbstractValidator<ChangeEventCommand>
        {
            public ChangeProductCommandValidator(DataImage images, DataSpace space)
            {
                RuleFor(c => c.Id).NotEmpty().WithMessage("Id: Изображения не существет"); 
                RuleFor(c => c.Name).NotEmpty().WithMessage("Name: Изображения не существет"); 
                RuleFor(c => c.Description).NotEmpty().WithMessage("Description: Изображения не существет"); 
                RuleFor(c => c.Begin).NotEmpty().WithMessage("Begin: Изображения не существет"); 
                RuleFor(c => c.End).NotEmpty().WithMessage("End: Изображения не существет"); 
                RuleFor(c => this.ImageCheck(images, c.ImageId)).Equal(true).WithMessage("ImageId: Изображения не существет");
                RuleFor(c => this.SpaceCheck(space, c.SpaceId)).Equal(true)
                    .WithMessage("SpaceId: Пространства не существет");
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

            private bool ImageCheck(DataImage images, Guid id)
            {
                var ch =  images.GetElementId(id);
                if (ch is null) return false;
                return true;
            }
            private bool SpaceCheck(DataSpace spaces, Guid id)
            {
                var ch =  spaces.GetElementId(id);
                if (ch is null) return false;
                return true;
            }
        }

    }
}
