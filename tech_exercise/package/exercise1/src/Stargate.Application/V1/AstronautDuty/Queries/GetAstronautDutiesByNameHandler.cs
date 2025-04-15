namespace Stargate.Application.V1.AstronautDuty.Queries;

using Microsoft.Extensions.Logging;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public class GetAstronautDutiesByNameHandler 
	: BaseHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
{
	public readonly IAstronautDutyRepository astronautDutyRepository;
	private readonly IPersonRepository personRepository;

	public GetAstronautDutiesByNameHandler(ILogger<GetAstronautDutiesByNameHandler> logger,
		IAstronautDutyRepository astronautDutyRepository,
		IPersonRepository personRepository) : base(logger)
	{
		this.astronautDutyRepository = astronautDutyRepository;
		this.personRepository = personRepository;
	}

	protected override async ValueTask<GetAstronautDutiesByNameResult> 
		HandleAsync(GetAstronautDutiesByName request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var person = await this.personRepository
			.GetPersonByNameAsync(request.Name, cancellationToken);

		if (person is null)
		{
			return new GetAstronautDutiesByNameResult
			{
				Message = $"Cannot find person with name of {request.Name}.",
				Success = false,
				ResponseCode = (int)HttpStatusCode.NotFound
			};
		}

		var duties = await this.astronautDutyRepository
			.GetAllAsync(request.Name, cancellationToken);

		var result = new GetAstronautDutiesByNameResult
		{
			Person = person.ToPerson(),
			AstronautDuties = duties.Select(d => d.ToPersonDuty(person)).ToList()
		};

		return result;
	}
}
