using FluentValidation.Results;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using SC.Internship.Common.Exceptions;
namespace MyService.Behaviours
{
    public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

   
      

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            if (failures.Count == 1)
                throw new ScException(failures.ElementAt(0).PropertyName + " " + failures.ElementAt(0).ErrorMessage);
            if (failures.Count != 0)
                throw new FluentValidation.ValidationException(failures);
           


            return await next();
        }
    }
}
