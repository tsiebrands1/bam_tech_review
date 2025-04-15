namespace Stargate.Application.V1.AstronautDuty.Queries;

using Stargate.Core.Dtos;
using System.Collections.Generic;

public class GetAstronautDutiesByNameResult : BaseResponse
{
	public PersonAstronaut Person { get; set; } = new();
	public IEnumerable<PersonDuty> AstronautDuties { get; set; } = [];
}
