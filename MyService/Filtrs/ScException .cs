using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using SC.Internship.Common.Exceptions;
using SC.Internship.Common.ScResult;

namespace MyService.Filtrs
{
    public class ScExceptionFiltr : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ScExceptionFiltr(IHostEnvironment hostEnvironment) =>
            _hostEnvironment = hostEnvironment;

        public void OnException(ExceptionContext context)
        {
            if (!_hostEnvironment.IsDevelopment())
            {
                return;
            }
            var c = context.Exception.GetType();
            var t = typeof(ScException);
            if (c == t)
                context.Result = new BadRequestObjectResult(new ScResult(new ScError { Message = context.Exception.Message }));
        }
    }
}
