namespace Stargate.Application.V1.Person.Queries;

using Microsoft.Extensions.Logging;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.Person;

public class GetPersonByNameHandler
	: BaseHandler<GetPersonByName, GetPersonByNameResult>
{
	private readonly IPersonRepository personRepository;
	public GetPersonByNameHandler(ILogger<GetPersonByNameHandler> logger,
		IPersonRepository personRepository) : base(logger)
	{
		this.personRepository = personRepository;
	}

	protected override async ValueTask<GetPersonByNameResult> HandleAsync(GetPersonByName request,
		CancellationToken cancellationToken)
	{
		var result = new GetPersonByNameResult();

		var query = $"SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate FROM [Person] a LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id WHERE '{request.Name}' = a.Name";

		var person = await this.personRepository
			.GetPersonByNameWithSqlAsync(query, cancellationToken) ??
				throw new EntityNotFoundException($"Cannot find person with name of {request.Name}.");

		result.Person = person.ToPerson(person.AstronautDetail);

		return result;
	}
}
