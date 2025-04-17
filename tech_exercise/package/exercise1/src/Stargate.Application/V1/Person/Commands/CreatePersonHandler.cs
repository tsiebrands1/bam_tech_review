namespace Stargate.Application.V1.Person.Commands;

using MediatR;
using Microsoft.Extensions.Logging;
using Stargate.Core.V1;
using Stargate.Core.V1.Person;
using System.Threading.Tasks;

public class CreatePersonHandler : BaseHandler<CreatePerson, CreatePersonResult>, IRequestHandler<CreatePerson, CreatePersonResult>
{
	private readonly IProcessWriteOperations<IPerson> personRepository;

	public CreatePersonHandler(ILogger<CreatePersonHandler> logger, 
		IProcessWriteOperations<IPerson> personRepository)
		: base(logger)
	{
		this.personRepository = personRepository;
	}

	protected override async ValueTask<CreatePersonResult> HandleAsync(CreatePerson request, CancellationToken cancellationToken)
	{
		var newPerson = request.ToPerson();

		newPerson.Id = await this.personRepository.AddOrUpdateEntityAsync(newPerson, cancellationToken);

		return new CreatePersonResult()
		{
			Id = newPerson.Id
		};
	}
}

