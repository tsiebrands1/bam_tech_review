namespace Stargate.Core.V1.AstronautDuty;

public interface IAstronautDutyRepository
{
	ValueTask<IEnumerable<IAstronautDuty>> GetAllAsync(string name, CancellationToken cancellationToken);
	ValueTask<IAstronautDuty?> GetAsync(int id, CancellationToken cancellationToken);
	ValueTask<IAstronautDuty?> GetByTitleAndStartDateAsync(string title, DateTime startDate,
		CancellationToken cancellationToken);
	ValueTask<IAstronautDuty?> GetByPersonIdAsync(int personId, CancellationToken cancellationToken);
	ValueTask<IEnumerable<IAstronautDuty>> GetCollectionByNameWithRawSqlAsync(string name,
		CancellationToken cancellationToken);
}
