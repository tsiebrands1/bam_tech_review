namespace Stargate.Core.V1.AstonautDetail;

using Stargate.Core.V1.Person;
using System;

public interface IAstronautDetail : IEntity
{
	DateTime? CareerEndDate { get; set; }
	DateTime CareerStartDate { get; set; }
	string CurrentDutyTitle { get; set; }
	string CurrentRank { get; set; }
	IPerson? Person { get; }
	int? PersonId { get; set; }

	void SetPerson(IPerson person);
}