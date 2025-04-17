namespace Stargate.Application.V1.Person;

using Stargate.Application.V1.Person.Commands;
using Stargate.Core.Dtos;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.Person;
using PersonEntity = Stargate.Data.Entities.Person;

public static class MapperExtensions
{
	public static IPerson ToPerson(this CreatePerson person)
	{
		return new PersonEntity
		{
			Name = person.Name
		};
	}

	public static PersonAstronaut ToPerson(this IPerson source, IAstronautDetail astronautDetail)
	{
		return new PersonAstronaut
		{
			CareerEndDate = astronautDetail.CareerEndDate,
			CareerStartDate = astronautDetail.CareerStartDate,
			CurrentDutyTitle = astronautDetail.CurrentDutyTitle,
			CurrentRank = astronautDetail.CurrentRank,
			Name = source.Name
		};
	}
}
