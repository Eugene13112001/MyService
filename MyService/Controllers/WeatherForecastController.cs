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
        /// �������� ��� �������
        /// </summary>
        ///  <remarks>
        /// �� ���� ������ �� ���������
        /// 
        /// ������ ���������� ������ ������ ���� ������� � ����
        /// </remarks>
        /// <returns>������ ���� �������</returns>
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
        /// �������� ������ �������, ������� �������� � ������ ���������� �������
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� ��� ���������  :
        /// 
        /// begin ���� datetime - ������ �������
        /// 
        /// end ���� datetime - ����� �������
        /// 
        /// ��������:
        /// 
        /// begin: 2023-03-14T22:02:57.704Z - ������ ������
        /// 
        /// end: 2023-04-14T22:02:57.704Z - ����� �������
        /// 
        /// ������ ������ �������, ������� �������� � ������ ���������� �������
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
        /// �������� ������� �� ��� id
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� �������� id ������� ���� Guid
        /// 
        /// ��������:
        /// 
        /// 3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// ������ ���������� ��� ���������� � �������, ���� ��� ����
        /// 
        /// � ���� ���, �� ���������� ��������� �� ����
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
            if (ev == null) return BadRequest("������� � ����� id ���");
            return new JsonResult(new { Event = ev });

        }
        /// <summary>
        /// �������� ����� ������� 
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� ������ �������:
        /// ��������:
        /// 
        /// {
        /// 
        /// "name": "string", - �������� �����������
        /// 
        ///"description": "string", - ��������
        ///
        ///"begin": "2023-03-14T21:57:07.693Z", - ���� ������
        ///
        /// "end": "2023-03-14T21:57:07.693Z", - ���� �����
        /// 
        /// "imageId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - id �����������
        /// 
        ///"spaceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6" - id ������������
        ///
        ///"count" - ���������� �������
        ///
        /// "free": true - ����� �� �������� ������
        ///
        /// }
        /// 
        /// ������ ���������� id,  ������ ������������ ������� � ���� ������ 
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
        /// �������� �������  
        /// </summary>
        ///  <remarks>
        /// �� ���� ����� ������ ������� ��������� id �������, ������� ����� ��������:
        /// 
        /// ��������:
        /// 
        /// 3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// ����� �� ���� ����� body ���������� ������ ������ �������:
        /// 
        /// ��������:
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
        /// "free": true - ����� �� �������� ������
        ///
        /// } 
        /// 
        /// ������ ���������� ���������� �������, ���� ��� ���� ������� ��������
        /// 
        /// ���� ���, �� ���������� ������� ������
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
            if (ev == null) return BadRequest("������� � ����� id ���");
            return new JsonResult(new { id = ev });

        }
        /// <summary>
        /// ������� ������� �� id 
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� id �������:
        /// 
        /// ��������:
        /// 
        /// 3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// ������ ���������� true, ���� ������� ���� ������� ������
        /// 
        /// ���� ���, �� ���������� ������� ������
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
            if (ev is false) return BadRequest("������� � ����� id ���");
            return new JsonResult(true);


        }
        /// <summary>
        /// ���� ������������ �����
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� :
        /// 
        /// {
        /// 
        /// "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - ������� �� ������� ����� ���� �����
        /// 
        /// 
        ///"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - ������������ ������� ����� ���� �����
        /// 
        /// }
        /// ������ ���������� �����, ���� �� ���� ������� �������
        /// 
        /// ���� ���, �� ���������� ������� ������
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
        /// ��������� ������� ������ � ������������
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� :
        /// 
        /// {
        /// 
        /// "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - ������� ��� �������� ����� ��������� �����
        /// 
        ///"userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - ������������ ��� �������� ����� ��������� �����
        /// 
        /// }
        /// ������ ���������� true,  ����� , ���� �� ����������
        /// 
        /// ���� ���, �� ���������� ������� null
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
        /// �������� ������ ��� �������
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� ��������� �����:
        /// 
        /// {
        /// 
        /// "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", - ������� ��� �������� ����� �������� �����
        /// 
        ///
        ///"count": 0 - ���������� ����������� �������
        /// 
        /// }
        /// ������ ���������� true, ���� ������ ���� ������� ���������
        /// 
        /// ���� ���, �� ���������� ������� ������
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
        /// ������ ��� ��������� ������ � ������
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� :
        /// 
        /// {
        /// 
        /// "password": Poltavka -������
        /// 
        ///"name": Eugene - �����
        /// 
        /// }
        /// ������ ���������� true, �������������� ��������� �������
        /// 
        /// ���� ���, �� ���������� ��� 401
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
            // �������� �����
            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:32768/WeatherForecast?username=Eugene&password=Poltavka'");
                // �������� �����
           string content = await response.Content.ReadAsStringAsync();
           
            
            return new JsonResult(new { Ticket = content });

        }
        /// <summary>
        /// ������ �������� ��������������
        /// </summary>
        ///  <remarks>
        /// �� ���� ��������� :
        /// 
        /// {
        /// 
        /// "token": JWT �����
  
        /// 
        /// }
        /// ������ ���������� true, �������������� ��������� �������
        /// 
        /// ���� ���, �� ���������� ��� 401
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