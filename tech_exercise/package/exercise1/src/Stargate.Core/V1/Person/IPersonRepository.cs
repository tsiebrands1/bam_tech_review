namespace Stargate.Core.V1.Person;
public interface IPersonRepository
{
	ValueTask<IEnumerable<IPerson>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<IPerson?> GetAsync(int id, CancellationToken cancellationToken);
	ValueTask<IPerson?> GetPersonByNameAsync(string name, CancellationToken cancellationToken);
	ValueTask<IPerson?> GetPersonByNameWithSqlAsync(string name, CancellationToken cancellationToken);
	ValueTask<IEnumerable<IPerson>> GetPeopleByAstronautDutyNameWithSqlAsync(string astronautDutyName, 
		CancellationToken cancellationToken);
	ValueTask<IEnumerable<IPerson>> GetPeopleWithSqlAsync(string query, CancellationToken cancellationToken);
}
