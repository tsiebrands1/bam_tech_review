namespace Stargate.Application.V1.Person.Commands;

using MediatR.Pipeline;
using Stargate.Core.V1;
using Stargate.Core.V1.Person;
using System.Threading.Tasks;

public class CreatePersonPreProcessor(IProcessWriteOperations<IPerson> personRepository) 
	: IRequestPreProcessor<CreatePerson>
{
	private readonly IProcessWriteOperations<IPerson> personRepository = personRepository;

	public async Task Process(CreatePerson request, CancellationToken cancellationToken)
	{
		var person = request.ToPerson();
		var result = await this.personRepository.AddOrUpdateEntityAsync
			(person, cancellationToken);
	}
}
