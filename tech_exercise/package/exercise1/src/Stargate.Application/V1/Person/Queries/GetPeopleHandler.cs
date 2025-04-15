namespace Stargate.Application.V1.Person.Queries;

using Microsoft.Extensions.Logging;
using Stargate.Core.V1.Person;
using System.Linq;
using System.Threading.Tasks;

public class GetPeopleHandler : BaseHandler<GetPeople, GetPeopleResult>
{
	public readonly IPersonRepository personRepository;

	public GetPeopleHandler(ILogger<GetPeopleHandler> logger,
		IPersonRepository personRepository) : base(logger)
	{
		this.personRepository = personRepository;
	}

	protected override async ValueTask<GetPeopleResult> HandleAsync
		(GetPeople request, CancellationToken cancellationToken)
	{
		var result = new GetPeopleResult();

		var query = $"SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate FROM [Person] a LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id";

		var people = await this.personRepository
			.GetPeopleWithSqlAsync(query, cancellationToken);

		result.People = people.Select(x => x.ToPerson(x.AstronautDetail)).ToList();

		return result;
	}
}
