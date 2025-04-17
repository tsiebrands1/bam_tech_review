namespace Stargate.Core.V1;

using System.Threading.Tasks;

public interface IProcessWriteOperations<TEntity>
	where TEntity : IEntity
{
	ValueTask<int> AddOrUpdateEntityAsync(TEntity entity, CancellationToken cancellationToken);
	ValueTask DeleteEntityAsync(TEntity entity, CancellationToken cancellationToken);
}
