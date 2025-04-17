namespace Stargate.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stargate.Core.Exceptions;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Data.Entities;
using Stargate.Data.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AstronautDetailRepository(
		ILogger<AstronautDetailRepository> logger,
		IDbContextFactory<StargateContext> dbContextFactory) 
	: BaseRepository<IAstronautDetail>(logger, dbContextFactory), 
		IAstronautDetailRepository
{
	protected override async ValueTask<int> AddOrUpdateAsync(IAstronautDetail entity,
		CancellationToken cancellationToken)
	{
		var existingEntity = (await this.StargateContext
			.AstronautDetails
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == entity.Id,
				cancellationToken));

		var astronautDetail = entity.ToAstronautDetail(existingEntity);

		if (existingEntity != null)
		{
			await this.StargateContext
				.AstronautDetails
				.Where(e => e.Id == astronautDetail.Id)
				.ExecuteUpdateAsync(setter => setter
					.SetProperty(x => x.CurrentRank,
						astronautDetail.CurrentRank), cancellationToken);
		}
		else
		{
			await this.StargateContext
				.AstronautDetails
				.AddAsync(astronautDetail, cancellationToken);
		}

		return astronautDetail.Id;
	}

	protected override async ValueTask DeleteAsync(IAstronautDetail entity, CancellationToken cancellationToken)
	{
		var existingEntity = this.StargateContext
			.AstronautDetails
			.FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken) ??
				throw new EntityNotFoundException($"AstronautDetail with ID {entity.Id} not found.");

		await this.StargateContext
			.AstronautDetails
			.Where(x => x.Id == entity.Id)
			.ExecuteDeleteAsync(cancellationToken);
	}

	public async ValueTask<IEnumerable<IAstronautDetail>> GetAllAsync
		(CancellationToken cancellationToken)
	{
		var details = await this.StargateContext
			.AstronautDetails
			.AsNoTracking()
			.ToListAsync(cancellationToken)
			.ConfigureAwait(false);

		return details
			.Select(e => e.FromEntity())
			.ToList();
	}

	public async ValueTask<IAstronautDetail?> GetAsync
		(int id, CancellationToken cancellationToken)
	{
		return await this.StargateContext
			.AstronautDetails
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<IAstronautDetail?> GetByPersonIdWithRawSqlAsync(int personId,
		CancellationToken cancellationToken)
	{
		var query = $"SELECT * FROM [AstronautDetail] WHERE {personId} = PersonId";

		return await this.StargateContext
			.AstronautDetails
			.FromSqlRaw(query)
			.AsNoTracking()
			.FirstOrDefaultAsync(cancellationToken)
			.ConfigureAwait(false);
	}
}
