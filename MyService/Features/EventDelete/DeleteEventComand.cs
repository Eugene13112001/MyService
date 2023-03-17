using FluentValidation;
using MediatR;
using MyService.Containers;
using MyService.Models;

namespace MyService.Features.EventDelete
{
    public class DeleteEventCommand : IRequest<bool>
    {

        public Guid Id { get; set; }

        public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, bool>
        {
            private readonly DataEvent _dataevent;
            private readonly DataImage _images;
            public readonly DataSpace _spaces;

            public DeleteEventCommandHandler(DataEvent events, DataImage images, DataSpace space)
            {
                this._spaces = space ?? throw new ArgumentNullException(nameof(space));
                this._dataevent = events ?? throw new ArgumentNullException(nameof(events));
                this._images = images ?? throw new ArgumentNullException(nameof(images));
            }

            public async Task<bool> Handle(DeleteEventCommand command, CancellationToken cancellationToken)
            {
                Event? ev = await this._dataevent.GetElementId(command.Id);
                if (ev is null) return false;
              
                return await this._dataevent.RemoveElement(ev); 
            }


        }
        public class AddProductCommandValidator : AbstractValidator<DeleteEventCommand>
        {
            public AddProductCommandValidator()
            {

                RuleFor(c => c.Id).NotEmpty();
                
            }

            
        }

    }
}
