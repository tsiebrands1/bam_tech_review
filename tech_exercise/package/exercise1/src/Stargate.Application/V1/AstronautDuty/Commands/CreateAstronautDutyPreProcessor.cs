namespace Stargate.Application.V1.AstronautDuty.Commands;

using MediatR.Pipeline;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using System;
using System.Threading.Tasks;

public class CreateAstronautDutyPreProcessor(IAstronautDutyRepository astronautDutyRepository,
	IPersonRepository personRepository) : IRequestPreProcessor<CreateAstronautDuty>
{
	private readonly IAstronautDutyRepository astronautDutyRepository =
		astronautDutyRepository;
	private readonly IPersonRepository personRepository = personRepository;

	public async Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		var person = await this.personRepository.GetPersonByNameAsync(request.Name, cancellationToken);

		if (person is null)
		{
			throw new EntityNotFoundException
				($"Cannot find person with name of {request.Name}.");
		}

		var verifyNoPreviousDuty = await this.astronautDutyRepository
			.GetByTitleAndStartDateAsync(request.DutyTitle, request.DutyStartDate,
			cancellationToken);

		if (verifyNoPreviousDuty is not null)
		{
			throw new BadHttpRequestException
				($"Previous duty exists for person with name of {request.Name}.");
		}
	}
}
