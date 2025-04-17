namespace Stargate.Core.V1.AstronautDuty;

using Stargate.Core.V1.Person;
using System;

public interface IAstronautDuty : IEntity
{
	DateTime? DutyEndDate { get; set; }
	DateTime DutyStartDate { get; set; }
	string DutyTitle { get; set; }
	IPerson? Person { get; }
	int? PersonId { get; set; }
	string Rank { get; set; }

	void SetPerson(IPerson person);
}