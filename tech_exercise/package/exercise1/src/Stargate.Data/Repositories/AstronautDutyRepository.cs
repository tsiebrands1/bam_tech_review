namespace Stargate.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Data.Entities;
using Stargate.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AstronautDutyRepository(
	ILogger<AstronautDutyRepository> logger,
	IDbContextFactory<StargateContext> dbContextFactory)
	: BaseRepository<IAstronautDuty>(logger, dbContextFactory), IAstronautDutyRepository
{
	protected override async ValueTask<int> AddOrUpdateAsync(IAstronautDuty entity,
		CancellationToken cancellationToken)
	{
		// Check if the entity already exists in the database
		var existingEntity = await this.StargateContext
			.AstronautDuties
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken);

		var astronautDuty = entity.ToAstronautDuty(existingEntity);

		if (existingEntity != null)
		{
			// Update the existing entity
			await this.StargateContext
				.AstronautDuties
				.Where(e => e.Id == astronautDuty.Id)
				.ExecuteUpdateAsync(setter => setter
					.SetProperty(e => e.DutyTitle, astronautDuty.DutyTitle)
					.SetProperty(e => e.DutyStartDate, astronautDuty.DutyStartDate)
					.SetProperty(e => e.DutyEndDate, astronautDuty.DutyEndDate)
					.SetProperty(e => e.Rank, astronautDuty.Rank), cancellationToken);
		}
		else
		{
			// Add the new entity
			await this.StargateContext.AstronautDuties.AddAsync(astronautDuty, cancellationToken);
		}

		return astronautDuty.Id;
	}

	protected override async ValueTask DeleteAsync(IAstronautDuty entity, CancellationToken cancellationToken)
	{
		var existingEntity = await this.StargateContext
			.AstronautDuties
			.FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken) ??
				throw new EntityNotFoundException($"AstronautDuty with ID {entity.Id} not found.");

		await this.StargateContext
			.AstronautDuties
			.Where(x => x.Id == entity.Id)
			.ExecuteDeleteAsync(cancellationToken);
	}

	public async ValueTask<IEnumerable<IAstronautDuty>> GetAllAsync
		(string name,CancellationToken cancellationToken)
	{
		return await this.StargateContext
			.AstronautDuties
			.Include(e => e.Person)
			.Where(e => e.Person != null && e.Person.Name == name)
			.AsNoTracking()
			.ToListAsync(cancellationToken).ConfigureAwait(false);
	}

	public async ValueTask<IEnumerable<IAstronautDuty>> GetCollectionByNameWithRawSqlAsync(string name, 
		CancellationToken cancellationToken)
	{
		var query = "SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, " +
			"b.CareerStartDate, b.CareerEndDate " +
			"FROM [Person] a LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id " +
			$"WHERE \'{name}\' = a.Name";

		return await this.StargateContext.AstronautDuties
			.FromSqlRaw(query)
			.AsNoTracking()
			.ToListAsync(cancellationToken)
			.ContinueWith(t => t.Result, cancellationToken);
	}

	public async ValueTask<IAstronautDuty?> GetAsync(int id,
		CancellationToken cancellationToken)
	{
		return await this.StargateContext
			.AstronautDuties
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == id, cancellationToken).ConfigureAwait(false)
				?? throw new EntityNotFoundException($"AstronautDuty with ID {id} not found.");
	}

	public async ValueTask<IAstronautDuty?> GetByPersonIdAsync(int personId,
		CancellationToken cancellationToken)
	{ 
		var person = await this.StargateContext
			.People
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == personId, cancellationToken)
			.ConfigureAwait(false);

		if(person is null)
		{
			throw new EntityNotFoundException($"Person with ID {personId} not found.");
		}

		return await this.StargateContext
			.AstronautDuties
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.PersonId == personId, cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<IAstronautDuty?> GetByTitleAndStartDateAsync(string title, 
		DateTime startDate, CancellationToken cancellationToken)
	{
		return await this.StargateContext
			.AstronautDuties
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.DutyTitle == title && e.DutyStartDate == startDate,
				cancellationToken)
			.ConfigureAwait(false);
	}
}
