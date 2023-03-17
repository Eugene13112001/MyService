using Microsoft.AspNetCore.Mvc;
using MediatR;
using MyService.Features.EventChange;
using MyService.Features.EventCreate;
using MyService.Features.EventDelete;
using MyService.Features.EventGet;
using MyService.Features.EventGetAll;
using MyService.Features.EventGetFiltr;
using MyService.Features.TicketCheck; 
using MyService.Features.TicketCreate;
using MyOwnServicw.Features.GiveTicket;
using MyService.Filtrs;
using MyService.Models;

namespace MyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {




        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// Получить все события
        /// </summary>
        ///  <remarks>
        /// На вход ничего не принимает
        /// Запрос возвращает полный список всех событий в базе
        /// </remarks>
        /// <returns>Список всех событий</returns>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/Events")]
        public async Task<IActionResult> GetAll()
        {

            GetAllEventCommand client = new GetAllEventCommand();
            CancellationToken token = new CancellationToken();
            List<Event> ev = await _mediator.Send(client, token);
            return new JsonResult(new { id = ev });


        }
        /// <summary>
        /// Получить список событий, который попадают в данный промежуток времени
        /// </summary>
        ///  <remarks>
        /// На вход принимает два параметра  :
        /// 
        /// begin типа datetime - начало периода
        /// 
        /// end типа datetime - конец периода
        /// 
        /// Например:
        /// 
        /// begin: 2023-03-14T22:02:57.704Z
        /// 
        /// end: 2023-04-14T22:02:57.704Z
        /// 
        /// Запрос список событий, которые попадают в данный промежуток времени
        /// </remarks>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/Events/{begin:datetime}&{end:datetime}")]
        public async Task<IActionResult> GetWithFiltr([FromRoute] DateTime begin, DateTime end)
        {


            GetAllEventFiltrCommand client = new GetAllEventFiltrCommand { Begin = begin, End = end };
            CancellationToken token = new CancellationToken();
            List<Event> ev = await _mediator.Send(client, token);
            return new JsonResult(new { id = ev });


        }
        /// <summary>
        /// Получить событие по его id
        /// </summary>
        ///  <remarks>
        /// На вход принимает параметр id типа Guid
        /// 
        /// Например:
        /// 
        /// 3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// Запрос возвращает всю информацию о событии, если оно есть
        /// А если нет, то возвращает сообщение об этом
        /// </remarks>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/Events/{id:guid}")]
        public async Task<IActionResult> GetEvent([FromRoute] Guid id)
        {

            GetEventCommand client = new GetEventCommand { Id = id };
            CancellationToken token = new CancellationToken();
            Event? ev = await _mediator.Send(client, token);
            if (ev == null) return BadRequest("События с таким id нет");
            return new JsonResult(new { Event = ev });

        }
        /// <summary>
        /// Добавить новое событие 
        /// </summary>
        ///  <remarks>
        /// На вход принимает шаблон события:
        /// Например:
        /// 
        /// {
        /// 
        /// "name": "string",
        /// 
        ///"description": "string",
        ///
        ///"begin": "2023-03-14T21:57:07.693Z",
        ///
        /// "end": "2023-03-14T21:57:07.693Z",
        /// 
        /// "imageId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        /// 
        ///"spaceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///
        /// }
        /// 
        /// Запрос возвращает id,  данный добавленному событию в базе данных 
        /// 
        /// </remarks>
        [Route("api/Events")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddEventCommand client,
           CancellationToken token)
        {


            Event ev = await _mediator.Send(client, token);
            return new JsonResult(new { id = ev.Id });

        }
        /// <summary>
        /// Изменить событие  
        /// </summary>
        ///  <remarks>
        /// На вход через строку запроса принимает id события, которое нужно изменить:
        /// 
        /// Например:
        /// 
        /// 3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// Также на вход через body передается шаблон нового события:
        /// 
        /// Например:
        /// 
        /// {
        /// 
        /// "name": "string",
        /// 
        ///"description": "string",
        ///"begin": "2023-03-14T21:57:07.693Z",
        ///
        /// "end": "2023-03-14T21:57:07.693Z",
        /// 
        /// "imageId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        /// 
        ///"spaceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///
        /// } 
        /// 
        /// Запрос возвращает измененное событие, если оно было успешно изменено
        /// 
        /// Если нет, то возвращает причину ошибки
        /// </remarks>

        [HttpPut]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/Events/{id:guid}")]
        public async Task<IActionResult> Change([FromRoute] Guid id, [FromBody] ChangeEventCommand client,
           CancellationToken token)
        {

            client.Id = id;
            Event? ev = await _mediator.Send(client, token);
            if (ev == null) return BadRequest("События с таким id нет");
            return new JsonResult(new { id = ev });

        }
        /// <summary>
        /// Удалить событие по id 
        /// </summary>
        ///  <remarks>
        /// На вход принимает id события:
        /// 
        /// Например:
        /// 
        /// 3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// Запрос возвращает true, если событие было успешно удален
        /// 
        /// Если нет, то возвращает причину ошибки
        /// </remarks>
        [HttpDelete]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/Events/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {

            DeleteEventCommand client = new DeleteEventCommand { Id = id };
            CancellationToken token = new CancellationToken();
            bool ev = await _mediator.Send(client, token);
            if (ev is false) return BadRequest("События с таким id нет");
            return new JsonResult(true);


        }
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/GiveTicket/{userid:guid}&{eventid:guid}")]
        public async Task<IActionResult> GiveTicket([FromRoute] Guid userid, Guid eventid)
        {

            GiveTicketCommand client = new GiveTicketCommand { UserId = userid, EventId = eventid };
            CancellationToken token = new CancellationToken();
            Ticket ev = await _mediator.Send(client, token);
            return new JsonResult(new { Ticket = ev });

        }
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/CheckTicket/{userid:guid}&{eventid:guid}")]
        public async Task<IActionResult> CheckTicket([FromRoute] Guid userid, Guid eventid)
        {

            CheckTicketCommand client = new CheckTicketCommand { UserId = userid, EventId = eventid };
            CancellationToken token = new CancellationToken();
            Ticket? ev = await _mediator.Send(client, token);
            return new JsonResult(new { Ticket = ev });

        }
        [HttpPost]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/AddTickets")]
        public async Task<IActionResult> AddTickets([FromBody] AddTicketCommand client, CancellationToken token)
        {

            
            bool ev = await _mediator.Send(client, token);
            return new JsonResult(new { result = ev });

        }
    }
}