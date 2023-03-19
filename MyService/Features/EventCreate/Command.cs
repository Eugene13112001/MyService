using FluentValidation;
using MediatR;
using System.Collections.Generic;
using MyService.Containers;
using MyService.Models;
namespace MyService.Features.EventCreate
{
    
        public class AddEventCommand: IRequest<Event>
        {

           
            public string Name { get; set; }

            public string Description { get; set; }
            public DateTime Begin { get; set; }

            public DateTime End { get; set; }

            public Guid ImageId { get; set; }

            public Guid SpaceId { get; set; }


            public  List<int> Places { get; set; }
            public bool Free { get; set; }
            public class AddEventCommandHandler : IRequestHandler<AddEventCommand, Event>
            {
                private readonly IData _dataevent;
                private readonly DataImage _images;
                public readonly DataSpace _spaces;

                public AddEventCommandHandler(IData events, DataImage images, DataSpace space)
                {
                        this._spaces = space ?? throw new ArgumentNullException(nameof(space));
                        this._dataevent = events ?? throw new ArgumentNullException(nameof(events));
                        this._images = images ?? throw new ArgumentNullException(nameof(images));
                }

                public async Task<Event> Handle(AddEventCommand command, CancellationToken cancellationToken)
                {
                    
                    Event ev = new Event();
                    ev.Name = command.Name;
                    ev.Free = command.Free;
                    ev.Description = command.Description;
                    ev.Begin = command.Begin;
                    ev.End = command.End;
                    ev.ImageId = command.ImageId;
                    ev.SpaceId = command.SpaceId;
                    
                    
                    Event newev =  await this._dataevent.AddEvent(ev);
                    return await this._dataevent.AddTickets(command.Places.Count, ev.Id, ev,  command.Places);
            }


            }
        public class AddProductCommandValidator : AbstractValidator<AddEventCommand>
        {
            public AddProductCommandValidator(DataImage images, DataSpace space)
            {

                RuleFor(c => this.NameCheck(c.Name)).Equal(true).WithMessage("Name: Имя не существет");
                RuleFor(c => this.NameCheckLength(c.Name)).Equal(true).WithMessage("Name: Длина имени маленькая"); 
                RuleFor(c => c.Description).NotEmpty().WithMessage("Description: Изображения не длина");
               
    
                RuleFor(c => c.Begin).NotEmpty().WithMessage("Begin: Даты начала нет");
                RuleFor(c => c.End).NotEmpty().WithMessage("End: Даты конца нет ");
                RuleFor(c => this.ImageCheck(images, c.ImageId)).Equal(true).WithMessage("ImageId: Пространства не существет");
                RuleFor(c => this.SpaceCheck(space, c.SpaceId)).Equal(true)
                    .WithMessage("SpaceId: Пространства не существет");
                RuleFor(c => this.DateCheck(c.Begin, c.End)).Equal(true)
                    .WithMessage("Date: Дата конца дожна быть больше даты начала");
               
            }

            private bool DateCheck(DateTime? begin, DateTime? end)
            {
                if (begin is null) return false;
                if (end is null) return false;
                if (begin <= end)return true;
                return false;
            }
            private bool NameCheck(string name)
            {
                if (name == "")
                    return false;
                return true;
            }
            private bool NameCheckLength(string name)
            {
                if (name.Length < 2)
                    return false;
                return true;
            }
            private bool ImageCheck(DataImage images,  Guid id )
            {
                var ch =  images.GetElementId(id);
                if (ch is null ) return false;
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
