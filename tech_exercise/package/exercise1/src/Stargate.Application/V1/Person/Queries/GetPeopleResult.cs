namespace Stargate.Application.V1.Person.Queries;

using Stargate.Core.Dtos;
using System.Collections.Generic;

public class GetPeopleResult : BaseResponse
{
	public List<PersonAstronaut> People { get; set; } = [];

}
