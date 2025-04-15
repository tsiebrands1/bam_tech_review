namespace Stargate.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stargate.Core.V1;
using Stargate.Data.Entities;
using System.Threading;

public abstract class BaseRepository<TEntity>(
	ILogger logger,
	IDbContextFactory<StargateContext> dbContextFactory) : IDisposable, IProcessWriteOperations<TEntity>
	where TEntity : class, IEntity
{
	private bool disposedValue;

	protected ILogger Logger { get; } = logger;

	protected StargateContext StargateContext { get; } = dbContextFactory.CreateDbContext();

	protected abstract ValueTask<int> AddOrUpdateAsync
		(TEntity entity, CancellationToken cancellationToken);

	public async ValueTask<int> AddOrUpdateEntityAsync
		(TEntity entity, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		try
		{	
			return await this.AddOrUpdateAsync(entity, cancellationToken);
		}
		catch (DbUpdateConcurrencyException ex)
		{
			this.Logger.LogError(ex, "Concurrency error occurred while adding or updating AstronautDuty.");
			throw;
		}
		catch (DbUpdateException ex)
		{
			this.Logger.LogError(ex, "Database update error occurred while adding or updating AstronautDuty.");
			throw;
		}
		catch (Exception ex)
		{
			this.Logger.LogError(ex, "An unexpected error occurred while adding or updating AstronautDuty.");
			throw;
		}
		finally
		{
			await this.StargateContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			this.Dispose(true);
		}
	}

	protected abstract ValueTask DeleteAsync(TEntity entity, CancellationToken cancellationToken);

	public async ValueTask DeleteEntityAsync(TEntity entity, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));
		try
		{
			await this.DeleteAsync(entity, cancellationToken);
		}
		catch (DbUpdateConcurrencyException ex)
		{
			this.Logger.LogError(ex, "Concurrency error occurred while deleting AstronautDuty.");
			throw;
		}
		catch (DbUpdateException ex)
		{
			this.Logger.LogError(ex, "Database update error occurred while deleting AstronautDuty.");
			throw;
		}
		catch (Exception ex)
		{
			this.Logger.LogError(ex, "An unexpected error occurred while deleting AstronautDuty.");
			throw;
		}
		finally
		{
			await this.StargateContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			this.Dispose(true);
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!this.disposedValue)
		{
			if (disposing)
			{
				this.StargateContext.Dispose();
			}
			this.disposedValue = true;
		}
	}

	void IDisposable.Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		this.Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
