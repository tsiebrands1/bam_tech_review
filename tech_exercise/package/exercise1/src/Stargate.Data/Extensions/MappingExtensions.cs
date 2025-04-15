namespace Stargate.Data.Extensions;

using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Entities;

public static class MappingExtensions
{
	public static Person ToPerson(this IPerson source, Person? destination = null!)
	{
		var entity = destination ?? new Person();
		if (source.Id != 0)
		{
			entity.Id = source.Id;
		}
		entity.Name = source.Name;
		return entity;
	}

	public static AstronautDetail ToAstronautDetail(this IAstronautDetail source,
		AstronautDetail? destination = null)
	{
		var entity = destination ?? new AstronautDetail();
		if (source.Id != 0)
		{
			entity.Id = source.Id;
		}
		entity.CurrentRank = source.CurrentRank;
		entity.CurrentDutyTitle = source.CurrentDutyTitle;
		entity.CareerStartDate = source.CareerStartDate;
		entity.CareerEndDate = source.CareerEndDate;

		return entity;
	}

	public static IAstronautDetail FromEntity(this AstronautDetail source)
	{
		return new AstronautDetail
		{
			Id = source.Id,
			CurrentRank = source.CurrentRank,
			CurrentDutyTitle = source.CurrentDutyTitle,
			CareerStartDate = source.CareerStartDate,
			CareerEndDate = source.CareerEndDate
		};
	}

	public static AstronautDuty ToAstronautDuty(this IAstronautDuty source,
		AstronautDuty? destination = null!)
	{
		var entity = destination ?? new AstronautDuty
		{
			Rank = source.Rank,
			DutyTitle = source.DutyTitle,
			DutyStartDate = source.DutyStartDate
		};
		if (source.Id != 0)
		{
			entity.Id = source.Id;
		}
		entity.DutyEndDate = source.DutyEndDate;
		return entity;
	}
}
