namespace Stargate.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.Person;
using Stargate.Data.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StargateContext = Stargate.Data.Entities.StargateContext;

public class PersonRepository(
		ILogger<PersonRepository> logger,
		IDbContextFactory<StargateContext> dbContextFactory)
	: BaseRepository<IPerson>(logger, dbContextFactory), IPersonRepository
{
	protected override async ValueTask<int> AddOrUpdateAsync(IPerson entity,
		CancellationToken cancellationToken)
	{
		var existingEntity = await this.StargateContext
			.People
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken);

		var person = entity.ToPerson(existingEntity);

		if (existingEntity != null)
		{
			await this.StargateContext
				.People
				.Where(e => e.Id == person.Id)
				.ExecuteUpdateAsync(setter => setter
					.SetProperty(e => e.Name, person.Name), cancellationToken);
		}
		else
		{
			await this.StargateContext.People.AddAsync(person, cancellationToken);
		}

		return person.Id;
	}
	protected override async ValueTask DeleteAsync(IPerson entity,
		CancellationToken cancellationToken)
	{ 
		var existingEntity = await this.StargateContext
			.People
			.FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken) ??
				throw new EntityNotFoundException($"Person with ID {entity.Id} not found.");

		await this.StargateContext
			.People
			.Where(x => x.Id == entity.Id)
			.ExecuteDeleteAsync(cancellationToken);
	}

	public async ValueTask<IEnumerable<IPerson>> GetAllAsync
		(CancellationToken cancellationToken)
	{ 
		return await this.StargateContext.People
			.AsNoTracking()
			.ToListAsync(cancellationToken);
	}

	public async ValueTask<IPerson?> GetAsync(int id,
		CancellationToken cancellationToken)
	{
		return await this.StargateContext
			.People
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
	}

	public async ValueTask<IEnumerable<IPerson>> GetPeopleByAstronautDutyNameWithSqlAsync
		(string astronautDutyName, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(astronautDutyName))
		{
			throw new ArgumentException(astronautDutyName, nameof(astronautDutyName));
		}
		var query = "SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate " +
			"FROM [Person] a LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id " +
			$"WHERE \'{astronautDutyName}\' = a.Name";

		var people = await this.StargateContext
			.Database.ExecuteSqlRawAsync(query, cancellationToken)
			.ContinueWith(t =>
			{
				if (t.IsFaulted)
				{
					throw new Exception("Error executing SQL query", t.Exception);
				}
				return this.StargateContext.People
					.AsNoTracking()
					.ToListAsync(cancellationToken);
			}, cancellationToken).ConfigureAwait(false);

		return (IEnumerable<IPerson>)people;
	}

	public async ValueTask<IPerson?> GetPersonByNameAsync(string name, 
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException(name, nameof(name));
		}

		return await this.StargateContext.People.FirstOrDefaultAsync(x =>
			string.Equals(name, x.Name, StringComparison.OrdinalIgnoreCase), 
			cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask<IPerson?> GetPersonByNameWithSqlAsync(string name,
		CancellationToken cancellationToken)
	{
		var query = $"SELECT * FROM [Person] WHERE \'{name}\' = Name";

		return await this.StargateContext.People
			.FromSqlRaw(query)
			.AsNoTracking()
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<IEnumerable<IPerson>> GetPeopleWithSqlAsync(string query,
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(query))
		{
			throw new ArgumentException(query, nameof(query));
		}

		return await this.StargateContext.People
			.FromSqlRaw(query)
			.AsNoTracking()
			.ToListAsync(cancellationToken)
			.ConfigureAwait(false);
	}
}
