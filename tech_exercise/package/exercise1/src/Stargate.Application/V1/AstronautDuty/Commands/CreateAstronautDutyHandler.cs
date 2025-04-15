namespace Stargate.Application.V1.AstronautDuty.Commands;
using Microsoft.Extensions.Logging;
using Stargate.Core.Exceptions;
using Stargate.Core.V1;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using Stargate.Data.Extensions;
using System.Net;
using System.Threading.Tasks;

public class CreateAstronautDutyHandler
	: BaseHandler<CreateAstronautDuty, CreateAstronautDutyResult>
{
	private readonly IPersonRepository personRepository;
	private readonly IAstronautDetailRepository astronautDetailRepository;
	private readonly IProcessWriteOperations<IAstronautDetail> astronautDetailWriteOperations;
	private readonly IProcessWriteOperations<IAstronautDuty> astronautDutyWriteOperations;

	public CreateAstronautDutyHandler(ILogger<CreateAstronautDutyHandler> logger,
		IPersonRepository personRepository,
		IAstronautDetailRepository astronautDetailRepository,
		IProcessWriteOperations<IAstronautDetail> astronautDetailWriteOperations,
		IProcessWriteOperations<IAstronautDuty> astronautDutyWriteOperations)
		: base(logger)
	{
		this.personRepository = personRepository;
		this.astronautDetailRepository = astronautDetailRepository;
		this.astronautDetailWriteOperations = astronautDetailWriteOperations;
		this.astronautDutyWriteOperations = astronautDutyWriteOperations;
	}

	protected override async ValueTask<CreateAstronautDutyResult> HandleAsync(CreateAstronautDuty request,
		CancellationToken cancellationToken)
	{
		var person = await this.personRepository
			.GetPersonByNameWithSqlAsync(request.Name, cancellationToken) ??
				throw new EntityNotFoundException($"Cannot find person with name of {request.Name}.");

		var astronautDetail = await this.astronautDetailRepository
			.GetByPersonIdWithRawSqlAsync(person.Id, cancellationToken);

		var astronautDetailEntity = request.ToAstronautDetail(person, astronautDetail);

		await this.astronautDetailWriteOperations.AddOrUpdateEntityAsync(astronautDetailEntity, cancellationToken);

		var astronautDuty = request.ToAstronautDuty();

		var result = await this.astronautDutyWriteOperations
			.AddOrUpdateEntityAsync(astronautDuty, cancellationToken);

		return new CreateAstronautDutyResult()
		{
			Id = result,
			Success = true,
			Message = $"Astronaut duty, {request.DutyTitle}, created successfully for {person.Name}.",
			ResponseCode = (int)HttpStatusCode.Created
		};
	}
}
