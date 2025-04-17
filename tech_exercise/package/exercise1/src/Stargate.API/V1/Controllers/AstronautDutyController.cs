namespace Stargate.API.V1.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Application.V1;
using Stargate.Application.V1.Person.Queries;
using Stargate.Core.Dtos;
using System.Net;

[ApiController]
[Route("[controller]")]
public class AstronautDutyController(IMediator mediator) : ControllerBase
{
	private readonly IMediator mediator = mediator;

	[HttpGet("{name}")]
	public async Task<IActionResult> GetAstronautDutiesByName(string name)
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
	public async Task<IActionResult> CreateAstronautDuty([FromBody] PersonAstronaut request)
	{
		try
		{
			var createAstronautDutyRequest = request.ToCreateAstronautDuty();
			var result = await this.mediator.Send(createAstronautDutyRequest);
			return this.GetResponse(result);
		}
		catch (Exception ex)
		{
			return this.GetResponse(new BaseResponse()
			{
				Message = ex.ToString(),
				Success = false,
				ResponseCode = (int)HttpStatusCode.InternalServerError
			});
		}
	}
}