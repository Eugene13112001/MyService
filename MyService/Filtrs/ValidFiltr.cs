using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using SC.Internship.Common.ScResult;
using MediatR.NotificationPublishers;

namespace MyService.Filtrs
{
    public class SampleExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        public SampleExceptionFilter(IHostEnvironment hostEnvironment) =>
            _hostEnvironment = hostEnvironment;

        public void OnException(ExceptionContext context)
        {
            if (!_hostEnvironment.IsDevelopment())
            {
                return;
            }
            var c = context.Exception.GetType();
            var t = typeof(FluentValidation.ValidationException);

            if (c == t)
            {
                var excep = (FluentValidation.ValidationException)context.Exception;
                Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
                foreach (var i in  excep.Errors)
                {
                    string str = i.ErrorMessage;
                    string[] words = str.Split(':');
                    if (!dict.ContainsKey(words[0])) dict[words[0]]= new List<string> { words[1] };


                }
                context.Result = new BadRequestObjectResult(new ScResult(new ScError { ModelState = dict }));

            }
            
            
        }
    }
}
