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
using DalSoft.RestClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;

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
        /// 
        /// Запрос возвращает полный список всех событий в базе
        /// </remarks>
        /// <returns>Список всех событий</returns>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [TypeFilter(typeof(ScExceptionFiltr))]
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
        /// begin: 2023-03-14T22:02:57.704Z - начала приода
        /// 
        /// end: 2023-04-14T22:02:57.704Z - конец периода
        /// 
        /// Запрос список событий, которые попадают в данный промежуток времени
        /// </remarks>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [TypeFilter(typeof(ScExceptionFiltr))]
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
        /// На вход принимает параметр id события типа Guid
        /// 
        /// Например:
        /// 
        /// 3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// Запрос возвращает всю информацию о событии, если оно есть
        /// 
        /// А если нет, то возвращает сообщение об этом
        /// </remarks>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [TypeFilter(typeof(ScExceptionFiltr))]
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
        /// "name": "string", - название мероприятия
        /// 
        ///"description": "string", - описание
        ///
        ///"begin": "2023-03-14T21:57:07.693Z", - дата начала
        ///
        /// "end": "2023-03-14T21:57:07.693Z", - дата конца
        /// 
        /// "imageId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - id изображения
        /// 
        ///"spaceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6" - id пространства
        ///
        ///"count" - количество билетов
        ///
        /// "free": true - можно ли добалять билеты
        ///
        /// }
        /// 
        /// Запрос возвращает id,  данный добавленному событию в базе данных 
        /// 
        /// </remarks>
        [Route("api/Events")]
        [TypeFilter(typeof(ScExceptionFiltr))]
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
        ///
        ///"begin": "2023-03-14T21:57:07.693Z",
        ///
        /// "end": "2023-03-14T21:57:07.693Z",
        /// 
        /// "imageId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        /// 
        ///"spaceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///
        /// "free": true - можно ли добалять билеты
        ///
        /// } 
        /// 
        /// Запрос возвращает измененное событие, если оно было успешно изменено
        /// 
        /// Если нет, то возвращает причину ошибки
        /// </remarks>

        [HttpPut]
        [TypeFilter(typeof(ScExceptionFiltr))]
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
        [TypeFilter(typeof(ScExceptionFiltr))]
        [Route("api/Events/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {

            DeleteEventCommand client = new DeleteEventCommand { Id = id };
            CancellationToken token = new CancellationToken();
            bool ev = await _mediator.Send(client, token);
            if (ev is false) return BadRequest("События с таким id нет");
            return new JsonResult(true);


        }
        /// <summary>
        /// Дать пользователю билет
        /// </summary>
        ///  <remarks>
        /// На вход принимает :
        /// 
        /// {
        /// 
        /// "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - событие на которое нужно дать билет
        /// 
        /// 
        ///"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - пользователь котрому нужно дать билет
        /// 
        /// }
        /// Запрос возвращает билет, если ое было успешно подарен
        /// 
        /// Если нет, то возвращает причину ошибки
        /// </remarks>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [TypeFilter(typeof(ScExceptionFiltr))]
        [Route("api/GiveTicket/{userid:guid}&{eventid:guid}")]
        public async Task<IActionResult> GiveTicket([FromRoute] Guid userid, Guid eventid)
        {

            GiveTicketCommand client = new GiveTicketCommand { UserId = userid, EventId = eventid };
            CancellationToken token = new CancellationToken();
            Ticket ev = await _mediator.Send(client, token);
            return new JsonResult(new { Ticket = ev });

        }
        /// <summary>
        /// Проверить наличие билета у пользователя
        /// </summary>
        ///  <remarks>
        /// На вход принимает :
        /// 
        /// {
        /// 
        /// "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - событие для которого нужно проверить билет
        /// 
        ///"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - пользователь для которого нужно проверить билет
        /// 
        /// }
        /// Запрос возвращает true,  билет , если он существует
        /// 
        /// Если нет, то возвращает причину null
        /// </remarks>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [TypeFilter(typeof(ScExceptionFiltr))]
        [Route("api/CheckTicket/{userid:guid}&{eventid:guid}")]
        public async Task<IActionResult> CheckTicket([FromRoute] Guid userid, Guid eventid)
        {

            CheckTicketCommand client = new CheckTicketCommand { UserId = userid, EventId = eventid };
            CancellationToken token = new CancellationToken();
            Ticket? ev = await _mediator.Send(client, token);
            return new JsonResult(new { Ticket = ev });

        }
        /// <summary>
        /// Добавить билеты для события
        /// </summary>
        ///  <remarks>
        /// На вход принимает следующую форму:
        /// 
        /// {
        /// 
        /// "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - событие для которого нужно добавить билет
        /// 
        ///
        ///"count": 0 - количество добавляемых билетов
        /// 
        /// }
        /// Запрос возвращает true, если билеты были успешно добавлены
        /// 
        /// Если нет, то возвращает причину ошибки
        /// </remarks>
        [HttpPost]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [TypeFilter(typeof(ScExceptionFiltr))]
        [Route("api/AddTickets")]
        public async Task<IActionResult> AddTickets([FromBody] AddTicketCommand client, CancellationToken token)
        {

            
            bool ev = await _mediator.Send(client, token);
            return new JsonResult(new { result = ev });

        }
        /// <summary>
        /// Запрос для получения логина и пароля
        /// </summary>
        ///  <remarks>
        /// На вход принимает :
        /// 
        /// {
        /// 
        /// "password": Poltavka -пароль
        /// 
        ///"name": Eugene - логин
        /// 
        /// }
        /// Запрос возвращает true, аутентификация произошла успешно
        /// 
        /// Если нет, то возвращает код 401
        /// </remarks>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [TypeFilter(typeof(ScExceptionFiltr))]
        [Route("api/GetToken/{name}&{password}")]
        public async Task<IActionResult> GetToken([FromRoute] string password, string name)
        {

            HttpClient httpClient = new HttpClient();
            var client = new RestClient("http://myserviceserver").Get();
            
            var client3 = new RestClient("http://myserviceserver/WeatherForecast?username=Eugene&password=Poltavka").Get();
            // получаем ответ
            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:32768/WeatherForecast?username=Eugene&password=Poltavka'");
                // получаем ответ
           string content = await response.Content.ReadAsStringAsync();
           
            
            return new JsonResult(new { Ticket = content });

        }
        /// <summary>
        /// Запрос проверки аутентификации
        /// </summary>
        ///  <remarks>
        /// На вход принимает :
        /// 
        /// {
        /// 
        /// "token": JWT токен
  
        /// 
        /// }
        /// Запрос возвращает true, аутентификация произошла успешно
        /// 
        /// Если нет, то возвращает код 401
        [HttpGet]
        [Authorize]
        [TypeFilter(typeof(ScExceptionFiltr))]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("stub/authstub")]
        public IActionResult  CheckAuthoriz([FromHeader]
           string Authorization)
        {
            if (!this.tokencheck(Authorization)) return Unauthorized(("Invalide token"));

            return new JsonResult(true );

        }
        private bool tokencheck(string? tok)
        {
            if (tok is  null) return false;
            return true;
        }
    }
}