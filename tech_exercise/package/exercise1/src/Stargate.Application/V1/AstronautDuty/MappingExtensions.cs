namespace Stargate.Application.V1.AstronautDuty;

using Stargate.Application.V1.AstronautDuty.Commands;
using Stargate.Core.Dtos;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;
using AstronautDuty = Stargate.Data.Entities.AstronautDuty;

public static class MappingExtensions
{
	public static PersonDuty ToPersonDuty(this IAstronautDuty source, IPerson person)
	{
		ArgumentNullException.ThrowIfNull(source, nameof(source));
		ArgumentNullException.ThrowIfNull(person, nameof(person));

		return new PersonDuty
		{
			CareerEndDate = source.DutyEndDate,
			CareerStartDate = source.DutyStartDate,
			CurrentDutyTitle = source.DutyTitle,
			CurrentRank = source.Rank,
			PersonId = person.Id,
			Name = person.Name
		};
	}
	public static PersonAstronaut ToPerson(this IPerson person)
	{ 
		ArgumentNullException.ThrowIfNull(person, nameof(person));

		return new PersonAstronaut 
		{
			PersonId = person.Id,
			CareerEndDate = person.AstronautDetail.CareerEndDate,
			CareerStartDate = person.AstronautDetail.CareerStartDate,
			CurrentDutyTitle = person.AstronautDetail.CurrentDutyTitle,
			CurrentRank = person.AstronautDetail.CurrentRank,
			Name = person.Name
		};
	}

	public static IAstronautDuty ToAstronautDuty(this CreateAstronautDuty astronautDuty)
	{
		ArgumentNullException.ThrowIfNull(astronautDuty);

		return new AstronautDuty
		{
			DutyStartDate = astronautDuty.DutyStartDate,
			DutyEndDate = null,
			DutyTitle = astronautDuty.DutyTitle,
			Rank = astronautDuty.Rank
		};
	}

	public static IAstronautDetail ToAstronautDetail(this CreateAstronautDuty astronautDuty,
		IPerson person, IAstronautDetail? destination = null)
	{
		ArgumentNullException.ThrowIfNull(astronautDuty, nameof(astronautDuty));
		ArgumentNullException.ThrowIfNull(person, nameof(person));

		destination ??= new AstronautDetail();

		var isRetired = string.Equals(astronautDuty.DutyTitle, RETIRED,
			StringComparison.OrdinalIgnoreCase);

		if (isRetired && astronautDuty.DutyEndDate == null)
		{
			destination.CareerEndDate = DateTime.UtcNow.Date.AddDays(-1);
		}

		destination.CareerStartDate = astronautDuty.DutyStartDate;
		destination.CurrentRank = astronautDuty.Rank;
		destination.CurrentDutyTitle = astronautDuty.DutyTitle;
		destination.PersonId = person.Id;

		return destination;
	}
}
