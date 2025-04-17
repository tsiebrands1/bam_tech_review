namespace Stargate.Core.V1.Person;

using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using System.Collections.Generic;

public interface IPerson : IEntity
{
	IAstronautDetail AstronautDetail { get; set; }
	IEnumerable<IAstronautDuty> AstronautDuties { get; set; }
	string Name { get; set; }
}