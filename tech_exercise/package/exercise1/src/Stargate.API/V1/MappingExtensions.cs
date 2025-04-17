namespace Stargate.API.V1;

using Stargate.Application.V1.AstronautDuty.Commands;
using Stargate.Core.Dtos;

public static class MappingExtensions
{
	public static CreateAstronautDuty ToCreateAstronautDuty(this PersonAstronaut personAstronaut)
	{
		ArgumentNullException.ThrowIfNull(personAstronaut, nameof(personAstronaut));

		if(personAstronaut.CareerStartDate == null)
		{
			throw new ArgumentException("CareerStartDate cannot be null", nameof(personAstronaut));
		}

		return new CreateAstronautDuty(
			(DateTime)personAstronaut.CareerStartDate,
			personAstronaut.CareerEndDate,
			personAstronaut.Name,
			personAstronaut.CurrentRank,
			personAstronaut.CurrentDutyTitle);
	}
}
