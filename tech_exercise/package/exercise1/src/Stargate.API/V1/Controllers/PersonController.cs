namespace Stargate.API.V1.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Application.V1;
using Stargate.Application.V1.Person.Commands;
using Stargate.Application.V1.Person.Queries;
using System.Net;


[ApiController]
[Route("[controller]")]
public class PersonController(IMediator mediator) 
	: ControllerBase
{
	private readonly IMediator mediator = mediator;

	[HttpGet("")]
	public async Task<IActionResult> GetPeople()
	{
		try
		{
			var result = await this.mediator.Send(new GetPeople()
			{

			});

			return this.GetResponse(result);
		}
		catch (Exception ex)
		{
			return this.GetResponse(new BaseResponse()
			{
				Message = ex.Message,
				Success = false,
				ResponseCode = (int)HttpStatusCode.InternalServerError
			});
		}
	}

	[HttpGet("{name}")]
	public async Task<IActionResult> GetPersonByName(string name)
	{
		try
		{
			var result = await this.mediator.Send(new GetPersonByName()
			{
				Name = name
			});

			return this.GetResponse(result);
		}
		catch (Exception ex)
		{
			return this.GetResponse(new BaseResponse()
			{
				Message = ex.Message,
				Success = false,
				ResponseCode = (int)HttpStatusCode.InternalServerError
			});
		}
	}

	[HttpPost("")]
	public async Task<IActionResult> CreatePerson([FromBody] string name)
	{
		try
		{
			var result = await this.mediator.Send(new CreatePerson()
			{
				Name = name
			});

			return this.GetResponse(result);
		}
		catch (Exception ex)
		{
			return this.GetResponse(new BaseResponse()
			{
				Message = ex.Message,
				Success = false,
				ResponseCode = (int)HttpStatusCode.InternalServerError
			});
		}
	}
}